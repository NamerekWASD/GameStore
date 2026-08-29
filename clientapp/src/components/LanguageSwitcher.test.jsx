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

test('toggle shows the current language as an SVG flag and code, not an emoji', () => {
  render(<LanguageSwitcher />);

  expect(screen.getByText('EN')).toBeInTheDocument();
  const flags = document.querySelectorAll('.language-flag');
  expect(flags.length).toBeGreaterThan(0);
  flags.forEach(flag => expect(flag.tagName).toBe('IMG'));
});

test('marks the active language in the dropdown with a checkmark, not a bootstrap active class', () => {
  render(<LanguageSwitcher />);
  fireEvent.click(screen.getByText('EN'));

  const activeItem = screen.getByText('English').closest('.language-menu-item');
  const inactiveItem = screen.getByText('Українська').closest('.language-menu-item');

  expect(activeItem.querySelector('svg')).not.toBeNull();
  expect(activeItem.classList.contains('active')).toBe(false);
  expect(inactiveItem.querySelector('svg')).toBeNull();
});
