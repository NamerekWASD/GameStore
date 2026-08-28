import en from './locales/en.json';
import uk from './locales/uk.json';

// i18next plural suffixes (CLDR categories) vary per language - e.g. Ukrainian
// has one/few/many/other while English only has one/other for the same key.
// Strip them so plural variants collapse to their shared base key.
const PLURAL_SUFFIX = /_(zero|one|two|few|many|other)$/;

function flattenKeys(obj, prefix = '') {
  return Object.entries(obj).flatMap(([key, value]) => {
    const path = prefix ? `${prefix}.${key}` : key;
    if (value !== null && typeof value === 'object' && !Array.isArray(value)) {
      return flattenKeys(value, path);
    }
    return [path.replace(PLURAL_SUFFIX, '')];
  });
}

test('en.json and uk.json expose the same set of translation keys', () => {
  const enKeys = new Set(flattenKeys(en));
  const ukKeys = new Set(flattenKeys(uk));

  const missingInUk = [...enKeys].filter(key => !ukKeys.has(key));
  const missingInEn = [...ukKeys].filter(key => !enKeys.has(key));

  expect({ missingInUk, missingInEn }).toEqual({ missingInUk: [], missingInEn: [] });
});
