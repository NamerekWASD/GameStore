import i18n from 'i18next';

export function formatDate(value) {
  if (!value) return '';
  const date = value instanceof Date ? value : new Date(value);
  const options = {
    day: 'numeric',
    month: 'long',
    year: 'numeric',
    hour: 'numeric',
    minute: 'numeric',
    hour12: false
  };
  try {
    return new Intl.DateTimeFormat(i18n.language || 'uk-UA', options).format(date);
  } catch (e) {
    return date.toLocaleString();
  }
}

export function formatCurrency(value, currency = 'USD') {
  if (value == null) return '';
  try {
    return new Intl.NumberFormat(i18n.language || 'uk-UA', { style: 'currency', currency }).format(value);
  } catch (e) {
    return `${value} ${currency}`;
  }
}
