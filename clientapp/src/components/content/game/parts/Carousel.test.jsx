import React from 'react';
import { render, act } from '@testing-library/react';
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

  expect(container.querySelectorAll('.my-carousel-item')).toHaveLength(5);
  expect(container.querySelectorAll('.my-carousel-bullet')).toHaveLength(5);
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
