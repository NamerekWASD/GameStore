import React from "react";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faGithub, faInstagram, faLinkedin } from '@fortawesome/free-brands-svg-icons'
import { faBox, faEnvelope, faGamepad, faGem, faHome, faKey, faLink, faLock, faPercent, faPhone, faRocket } from "@fortawesome/free-solid-svg-icons";
import { AppPaths } from "../utils/AppPaths";
import { useNavigate } from "react-router-dom";
import { navigateToManager } from "../utils/Navigation";
import { useTranslation } from 'react-i18next';

const Footer = () => {
    const navigate = useNavigate();
    const { t } = useTranslation();

    const manager = () => {
        navigateToManager(navigate);
    }

    return (
        <footer className="text-center text-lg-start footer">
            <section className="d-flex justify-content-center p-4 border-bottom">
                <div className="me-3 d-none d-lg-block">
                    <span>{t('footer.joinUs')}</span>
                </div>
                <div>
                    <a href="" target="_blank" className="me-4 text-reset">
                        <FontAwesomeIcon icon={faInstagram} />
                    </a>
                    <a href="" target="_blank" className="me-4 text-reset">
                        <FontAwesomeIcon icon={faLinkedin} />
                    </a>
                    <a href="https://github.com/NamerekWASD/GameStore" target="_blank" className="me-4 text-reset">
                        <FontAwesomeIcon icon={faGithub} />
                    </a>
                </div>
            </section>
            <section className="">
                <div className="container text-center text-md-start mt-5">
                    <div className="row mt-3">
                        <div className="col-md-3 col-lg-4 col-xl-3 mx-auto mb-4">
                            <h6 className="text-uppercase fw-bold mb-4">
                                <FontAwesomeIcon icon={faGem} /> Namerek
                            </h6>
                            <strong>{t('footer.guaranteeTitle')}</strong>
                            <div className="d-flex flex-column gap-2">
                                <div className="d-flex flex-row align-items-center">
                                    <div className="me-2">
                                        <FontAwesomeIcon icon={faKey} className=" d-inline-block" />
                                    </div>
                                    <span className="line-height-normal">{t('footer.guarantee.item1')}</span>
                                </div>
                                <div className="d-flex flex-row align-items-center">
                                    <div className="me-2">
                                        <FontAwesomeIcon icon={faLock} />
                                    </div>
                                    <span className="line-height-normal">{t('footer.guarantee.item2')}</span>
                                </div>
                                <div className="d-flex flex-row align-items-center">
                                    <div className="me-2">
                                        <FontAwesomeIcon icon={faRocket} />
                                    </div>
                                    <span className="line-height-normal">{t('footer.guarantee.item3')}</span>
                                </div>
                                <div className="d-flex flex-row align-items-center">
                                    <div className="me-2">
                                        <FontAwesomeIcon icon={faPercent} />
                                    </div>
                                    <span className="line-height-normal">{t('footer.guarantee.item4')}</span>
                                </div>
                            </div>
                        </div>
                        <div className="col-md-2 col-lg-2 col-xl-2 mx-auto mb-4">
                            <h6 className="text-uppercase fw-bold mb-4">
                                <FontAwesomeIcon icon={faBox} /> {t('footer.productsTitle')}
                            </h6>
                            <p>
                                <a href={AppPaths.gameCatalog} className="text-reset nav-link"><FontAwesomeIcon icon={faGamepad} /> {t('footer.products.games')}</a>
                            </p>
                        </div>
                        <div className="col-md-3 col-lg-2 col-xl-2 mx-auto mb-4">
                            <h6 className="text-uppercase fw-bold mb-4">
                                <FontAwesomeIcon icon={faLink} /> {t('footer.linksTitle')}
                            </h6>
                            <p>
                                <a href="#" className="text-reset nav-link">FAQ</a>
                            </p>
                            <p>
                                <button className="btn text-dark nav-link w-100 text-start" onClick={manager}>{t('footer.links.manager')}</button>
                            </p>
                        </div>
                        <div className="col-md-4 col-lg-3 col-xl-3 mx-auto mb-md-0 mb-4">
                            <h6 className="text-uppercase fw-bold mb-4">Контакти</h6>
                            <p><FontAwesomeIcon icon={faHome} /><span className="ms-1"> {t('footer.contacts.address')}</span></p>
                            <p>
                                <a href="mailto:namerek.inc@gmail.com" className="text-reset"><FontAwesomeIcon icon={faEnvelope} /><span className="ms-1"> namerek.inc@gmail.com</span></a>
                            </p>
                            <p>
                                <a href="tel:+380639737449" className="text-reset"><FontAwesomeIcon icon={faPhone} /><span className="ms-1">{t('footer.contacts.phone')}</span></a>
                            </p>
                        </div>
                    </div>
                </div>
            </section>
            <div className="text-center p-4 copyright">
                © 2023 {t('footer.copyright')}
                <a className="text-reset fw-bold ms-1" href="https://GameStore.gg/">GameStore.gg</a>
            </div>
        </footer>
    )
}

export default Footer;