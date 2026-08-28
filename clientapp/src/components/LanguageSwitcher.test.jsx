import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import i18n from '../i18n/i18n';
import LanguageSwitcher from './LanguageSwitcher';

beforeEach(async () => {
  localStorage.clear();
  await i18n.changeLanguage('en');
});

test('switching to Ukrainian calls i18n.changeLanguage and persists to localStorage', async () => {
  const changeLanguageSpy = jest.spyOn(i18n, 'changeLanguage');
  render(<LanguageSwitcher />);

  fireEvent.click(screen.getByText('Українська'));

  await waitFor(() => expect(changeLanguageSpy).toHaveBeenCalledWith('uk'));
  expect(localStorage.getItem('i18nextLng')).toBe('uk');

  changeLanguageSpy.mockRestore();
});
