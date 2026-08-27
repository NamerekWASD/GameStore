import React, { useEffect, useState } from 'react';
import i18n from 'i18next';
import { Dropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap';

const availableLangs = [
  { code: 'en', label: 'EN', name: 'English', emoji: '🇬🇧' },
  { code: 'uk', label: 'UK', name: 'Українська', emoji: '🇺🇦' }
];

const LanguageSwitcher = () => {
  const [open, setOpen] = useState(false);
  const [current, setCurrent] = useState((i18n.language || 'en').slice(0, 2));

  useEffect(() => {
    const onChange = () => setCurrent((i18n.language || 'en').slice(0, 2));
    // i18next doesn't expose an official event emitter on the default import,
    // but it does provide `on` when initialized; guard for it.
    if (i18n && i18n.on) i18n.on('languageChanged', onChange);
    return () => { if (i18n && i18n.off) i18n.off('languageChanged', onChange); };
  }, []);

  const toggle = () => setOpen(o => !o);

  const changeLang = (lng) => {
    i18n.changeLanguage(lng);
    try { localStorage.setItem('i18nextLng', lng); } catch (e) { }
    setOpen(false);
  };

  return (
    <Dropdown isOpen={open} toggle={toggle} className="language-switcher ms-2">
      <DropdownToggle caret color="link" className="text-white btn-sm rounded-0 language-toggle d-flex align-items-center">
        <span className="me-2" aria-hidden>{(availableLangs.find(l => l.code === current) || availableLangs[0]).emoji}</span>
        <span className="fw-bold">{current ? current.toUpperCase() : 'EN'}</span>
      </DropdownToggle>
      <DropdownMenu end className="language-menu">
        {availableLangs.map(lang => (
          <DropdownItem key={lang.code} onClick={() => changeLang(lang.code)} active={lang.code === current} className="d-flex align-items-center">
            <span className="me-2">{lang.emoji}</span>
            <span>{lang.name}</span>
          </DropdownItem>
        ))}
      </DropdownMenu>
    </Dropdown>
  );
};

export default LanguageSwitcher;
