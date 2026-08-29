import React from 'react';
import { render, act, fireEvent } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
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
  expect(container.querySelectorAll('.my-carousel-bullet')).toHaveLength(2);
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
  expect(container.querySelectorAll('.my-carousel-bullet')).toHaveLength(7);
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

test('moves the track via CSS transform instead of jQuery, and updates it on bullet click', async () => {
  const games = Array.from({ length: 9 }, (_, i) => makeGame(i + 1));
  const { container } = renderCarousel(games);
  await flushEffects();

  const track = container.querySelector('.carousel-track');
  expect(track).not.toBeNull();
  expect(track.style.transform).toBe('translateX(-0%)');
  expect(container.querySelectorAll('.my-carousel-item[style*="left"]')).toHaveLength(0);

  const bullets = container.querySelectorAll('.my-carousel-bullet');
  fireEvent.click(bullets[2]);
  await flushEffects();

  expect(track.style.transform).toBe('translateX(-200%)');
  expect(bullets[2].classList.contains('active')).toBe(true);
  expect(bullets[0].classList.contains('active')).toBe(false);
});
