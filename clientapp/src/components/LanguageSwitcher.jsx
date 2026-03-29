import i18n from 'i18next';

const LanguageSwitcher = () => {
  const changeLang = (lng) => {
    i18n.changeLanguage(lng);
    try { localStorage.setItem('i18nextLng', lng); } catch (e) { }
  };

  return (
    <div className="language-switcher">
      <button className="btn btn-sm btn-link text-white" onClick={() => changeLang('uk')}>UK</button>
      <button className="btn btn-sm btn-link text-white" onClick={() => changeLang('en')}>EN</button>
    </div>
  );
};

export default LanguageSwitcher;
