import React from 'react';
import { render, act, fireEvent } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import '../../../../i18n/i18n';
import Carousel from './Carousel';

beforeEach(() => {
  global.fetch = jest.fn(() => Promise.resolve({ json: () => Promise.resolve([]) }));
});

// Carousel renders VideoContainer, which fetches its video list on mount;
// flush that pending state update so it doesn't leak an act() warning into later tests.
async function flushEffects() {
  await act(async () => {
    await Promise.resolve();
  });
}

function makeGame(id) {
  return {
    id,
    title: `Game ${id}`,
    price: 10,
    isAvailable: false,
    images: [{ type: { name: 'POSTER' }, path: `/poster-${id}.jpg` }],
  };
}

function renderCarousel(games) {
  return render(
    <MemoryRouter>
      <Carousel games={games} />
    </MemoryRouter>
  );
}

test('renders the expected number of slides and bullets for 6 games', async () => {
  const games = Array.from({ length: 6 }, (_, i) => makeGame(i + 1));
  const { container } = renderCarousel(games);
  await flushEffects();

  // 6 games chunked by 3 => 2 slides.
  expect(container.querySelectorAll('.my-carousel-item')).toHaveLength(2);
  expect(container.querySelectorAll('.carousel-bullet')).toHaveLength(2);
});

test('does not crash with an empty games array', async () => {
  expect(() => renderCarousel([])).not.toThrow();
  await flushEffects();
});

test('does not crash when a game has no poster image', async () => {
  const games = [{ id: 1, title: 'No poster', price: 10, isAvailable: false, images: [] }];
  expect(() => renderCarousel(games)).not.toThrow();
  await flushEffects();
});

test('renders every game across slides for 20 games, not capped at 5 slides', async () => {
  const games = Array.from({ length: 20 }, (_, i) => makeGame(i + 1));
  const { container } = renderCarousel(games);
  await flushEffects();

  // 20 games chunked by 3 => 7 slides (6 full + 1 partial), not the old hard cap of 5.
  expect(container.querySelectorAll('.my-carousel-item')).toHaveLength(7);
  expect(container.querySelectorAll('.carousel-bullet')).toHaveLength(7);
});

test.each([0, 1, 2, 3])('does not crash and renders correct slide count for %i games', async (count) => {
  const games = Array.from({ length: count }, (_, i) => makeGame(i + 1));
  const { container } = renderCarousel(games);
  await flushEffects();

  const expectedSlides = Math.ceil(count / 3);
  expect(container.querySelectorAll('.my-carousel-item')).toHaveLength(expectedSlides);
});

test('a game without a poster is skipped instead of crashing on undefined.path', async () => {
  const games = [
    { id: 1, title: 'No poster', price: 10, isAvailable: false, images: [] },
    makeGame(2),
    makeGame(3),
  ];
  const { container } = renderCarousel(games);
  await flushEffects();

  expect(container.querySelectorAll('img[alt="No poster"]')).toHaveLength(0);
  expect(container.querySelectorAll('img[alt="Game 2"]')).toHaveLength(1);
});

function mockMobileLayout() {
  const original = window.matchMedia;
  window.matchMedia = jest.fn().mockImplementation(query => ({
    matches: query === '(max-width: 800px)',
    media: query,
    addEventListener: () => { },
    removeEventListener: () => { },
  }));
  return () => { window.matchMedia = original; };
}

test('renders a hero card and two side cards per desktop slide', async () => {
  const games = Array.from({ length: 3 }, (_, i) => makeGame(i + 1));
  const { container } = renderCarousel(games);
  await flushEffects();

  expect(container.querySelectorAll('.carousel-hero-main')).toHaveLength(1);
  expect(container.querySelectorAll('.carousel-hero-side .carousel-card')).toHaveLength(2);
  expect(container.querySelectorAll('.carousel-card-gradient')).toHaveLength(3);
  expect(container.querySelectorAll('.carousel-card-title')).toHaveLength(3);
});

test('renders navigation arrows with accessible labels and they change the active slide', async () => {
  const games = Array.from({ length: 6 }, (_, i) => makeGame(i + 1));
  const { container, getByLabelText } = renderCarousel(games);
  await flushEffects();

  const track = container.querySelector('.carousel-track');
  fireEvent.click(getByLabelText('Next slide'));
  await flushEffects();
  expect(track.style.transform).toBe('translateX(-100%)');

  fireEvent.click(getByLabelText('Previous slide'));
  await flushEffects();
  expect(track.style.transform).toBe('translateX(-0%)');
});

test('exposes carousel region and bullet ARIA attributes', async () => {
  const games = Array.from({ length: 6 }, (_, i) => makeGame(i + 1));
  const { container, getByLabelText } = renderCarousel(games);
  await flushEffects();

  const region = container.querySelector('[role="region"]');
  expect(region).not.toBeNull();
  expect(region).toHaveAttribute('aria-roledescription', 'carousel');
  expect(region).toHaveAttribute('tabIndex', '0');

  expect(getByLabelText('Go to slide 1')).toHaveAttribute('aria-current', 'true');
});

test('ArrowRight/ArrowLeft keys move the slide when the carousel region has focus', async () => {
  const games = Array.from({ length: 6 }, (_, i) => makeGame(i + 1));
  const { container } = renderCarousel(games);
  await flushEffects();

  const region = container.querySelector('[role="region"]');
  const track = container.querySelector('.carousel-track');

  fireEvent.keyDown(region, { key: 'ArrowRight' });
  await flushEffects();
  expect(track.style.transform).toBe('translateX(-100%)');

  fireEvent.keyDown(region, { key: 'ArrowLeft' });
  await flushEffects();
  expect(track.style.transform).toBe('translateX(-0%)');
});

test('a left swipe advances to the next slide', async () => {
  const games = Array.from({ length: 6 }, (_, i) => makeGame(i + 1));
  const { container } = renderCarousel(games);
  await flushEffects();

  const wrapper = container.querySelector('.carousel-viewport-wrapper');
  const track = container.querySelector('.carousel-track');

  fireEvent.touchStart(wrapper, { touches: [{ clientX: 300 }] });
  fireEvent.touchEnd(wrapper, { changedTouches: [{ clientX: 200 }] });
  await flushEffects();

  expect(track.style.transform).toBe('translateX(-100%)');
});

test('autoplay pauses on mouse enter and resumes on mouse leave', async () => {
  jest.useFakeTimers();
  try {
    const games = Array.from({ length: 6 }, (_, i) => makeGame(i + 1));
    const { container } = renderCarousel(games);
    await act(async () => { await Promise.resolve(); });

    const region = container.querySelector('[role="region"]');
    const track = container.querySelector('.carousel-track');

    fireEvent.mouseEnter(region);
    act(() => { jest.advanceTimersByTime(6000); });
    expect(track.style.transform).toBe('translateX(-0%)');

    fireEvent.mouseLeave(region);
    act(() => { jest.advanceTimersByTime(5000); });
    expect(track.style.transform).toBe('translateX(-100%)');
  } finally {
    jest.useRealTimers();
  }
});

test('moves the track via CSS transform instead of jQuery, and updates it on bullet click', async () => {
  const games = Array.from({ length: 9 }, (_, i) => makeGame(i + 1));
  const { container } = renderCarousel(games);
  await flushEffects();

  const track = container.querySelector('.carousel-track');
  expect(track).not.toBeNull();
  expect(track.style.transform).toBe('translateX(-0%)');
  expect(container.querySelectorAll('.my-carousel-item[style*="left"]')).toHaveLength(0);

  const bullets = container.querySelectorAll('.carousel-bullet');
  fireEvent.click(bullets[2]);
  await flushEffects();

  expect(track.style.transform).toBe('translateX(-200%)');
  expect(bullets[2].classList.contains('active')).toBe(true);
  expect(bullets[0].classList.contains('active')).toBe(false);
});

test('mobile slide renders one full-width card with no JS-measured inline sizes', async () => {
  const restoreLayout = mockMobileLayout();
  try {
    const games = Array.from({ length: 3 }, (_, i) => makeGame(i + 1));
    const { container } = renderCarousel(games);
    await flushEffects();

    const cards = container.querySelectorAll('.carousel-slide .carousel-card');
    expect(cards).toHaveLength(3);
    expect(cards[0].classList.contains('carousel-mobile-main')).toBe(true);

    const image = cards[0].querySelector('img');
    expect(image.style.width).toBe('');
    expect(image.style.height).toBe('');
  } finally {
    restoreLayout();
  }
});

test('mobile layout puts one game per slide so no game is unreachable', async () => {
  const restoreLayout = mockMobileLayout();
  try {
    const games = Array.from({ length: 4 }, (_, i) => makeGame(i + 1));
    const { container } = renderCarousel(games);
    await flushEffects();

    expect(container.querySelectorAll('.my-carousel-item')).toHaveLength(4);
    expect(container.querySelectorAll('.carousel-bullet')).toHaveLength(4);
  } finally {
    restoreLayout();
  }
});
