import React from 'react';
import { render, waitFor } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import '@testing-library/jest-dom';
import App from './App';

// Plain functions, not jest.fn(): CRA's jest config sets resetMocks: true,
// which wipes mockImplementation() before each test runs.
jest.mock('./utils/ApiRequests', () => ({
  ...jest.requireActual('./utils/ApiRequests'),
  CheckAuthenticated: () => Promise.resolve(false),
  GetGenres: () => Promise.resolve([]),
}));

it('renders without crashing', async () => {
  render(
    <MemoryRouter>
      <App />
    </MemoryRouter>
  );

  await waitFor(() => {
    expect(document.getElementById('myHeader')).toBeInTheDocument();
  });
});
