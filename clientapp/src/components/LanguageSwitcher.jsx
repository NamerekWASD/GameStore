import React, { useEffect, useState } from 'react';
import i18n from 'i18next';
import { Dropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCheck } from '@fortawesome/free-solid-svg-icons';
import gbFlag from '../static/flags/gb.svg';
import uaFlag from '../static/flags/ua.svg';

const availableLangs = [
  { code: 'en', label: 'EN', name: 'English', flag: gbFlag },
  { code: 'uk', label: 'UK', name: 'Українська', flag: uaFlag }
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

  const activeLang = availableLangs.find(l => l.code === current) || availableLangs[0];

  return (
    <Dropdown isOpen={open} toggle={toggle} className="language-switcher">
      <DropdownToggle color="link" className="language-toggle d-flex align-items-center">
        <img src={activeLang.flag} alt="" className="language-flag" />
        <span className="fw-bold">{activeLang.label}</span>
      </DropdownToggle>
      <DropdownMenu end className="language-menu">
        {availableLangs.map(lang => (
          <DropdownItem key={lang.code} onClick={() => changeLang(lang.code)} className="d-flex align-items-center language-menu-item">
            <img src={lang.flag} alt="" className="language-flag me-2" />
            <span className="flex-fill">{lang.name}</span>
            {lang.code === current ? <FontAwesomeIcon icon={faCheck} className="ms-2" /> : null}
          </DropdownItem>
        ))}
      </DropdownMenu>
    </Dropdown>
  );
};

export default LanguageSwitcher;
