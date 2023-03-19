import React from "react";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faGithub, faInstagram, faLinkedin } from '@fortawesome/free-brands-svg-icons'
import { faBox, faEnvelope, faGamepad, faGem, faHome, faKey, faLink, faLock, faPercent, faPhone, faRocket } from "@fortawesome/free-solid-svg-icons";
import { AppPaths } from "../utils/AppPaths";
import { MANAGER } from "../utils/Constants";
import { useNavigate } from "react-router-dom";
import { navigateToManager } from "../utils/Navigation";

const Footer = () => {
    const navigate = useNavigate();

    

    return (
        <footer className="text-center text-lg-start text-muted footer">
            <section className="d-flex justify-content-center p-4 border-bottom">
                <div className="me-3 d-none d-lg-block">
                    <span>Приєднуйтесь до наших соціальних мереж:</span>
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
                            <strong>Ми гарантуємо:</strong>
                            <div className="d-flex flex-column gap-2">
                                <div className="d-flex flex-row align-items-center">
                                    <div className="me-2">
                                        <FontAwesomeIcon icon={faKey} className=" d-inline-block" />
                                    </div>
                                    <span className="line-height-normal">Ліцензійні ключі від офіційних видавців</span>
                                </div>
                                <div className="d-flex flex-row align-items-center">
                                    <div className="me-2">
                                        <FontAwesomeIcon icon={faLock} />
                                    </div>
                                    <span className="line-height-normal">Робочі аккаунти</span>
                                </div>
                                <div className="d-flex flex-row align-items-center">
                                    <div className="me-2">
                                        <FontAwesomeIcon icon={faRocket} />
                                    </div>
                                    <span className="line-height-normal">Гарантована техпідтримка вашої покупки</span>
                                </div>
                                <div className="d-flex flex-row align-items-center">
                                    <div className="me-2">
                                        <FontAwesomeIcon icon={faPercent} />
                                    </div>
                                    <span className="line-height-normal">Регулярні акції, знижки та бонуси</span>
                                </div>
                            </div>
                        </div>
                        <div className="col-md-2 col-lg-2 col-xl-2 mx-auto mb-4">
                            <h6 className="text-uppercase fw-bold mb-4">
                                <FontAwesomeIcon icon={faBox} /> Продукти
                            </h6>
                            <p>
                                <a href={AppPaths.gameCatalog} className="text-dark nav-link"><FontAwesomeIcon icon={faGamepad} /> Ігри</a>
                            </p>
                        </div>
                        <div className="col-md-3 col-lg-2 col-xl-2 mx-auto mb-4">
                            <h6 className="text-uppercase fw-bold mb-4">
                                <FontAwesomeIcon icon={faLink} /> Корисні посилання
                            </h6>
                            <p>
                                <a href="#" className="text-dark nav-link">FAQ</a>
                            </p>
                            <p>
                                <button className="btn text-dark nav-link" onClick={() => navigateToManager(navigate)}>Менеджеру</button>
                            </p>
                        </div>
                        <div className="col-md-4 col-lg-3 col-xl-3 mx-auto mb-md-0 mb-4">
                            <h6 className="text-uppercase fw-bold mb-4">Контакти</h6>
                            <p><FontAwesomeIcon icon={faHome} /><span className="ms-1"> вул. Ярославська, 56А, Київ, 04071</span></p>
                            <p>
                                <a href="mailto:namerek.inc@gmail.com" className="text-dark nav-link"><FontAwesomeIcon icon={faEnvelope} /><span className="ms-1"> namerek.inc@gmail.com</span></a>
                            </p>
                            <p>
                                <a href="tel:+380639737449" className="text-dark nav-link"><FontAwesomeIcon icon={faPhone} /><span className="ms-1">+38(063)973-74-49</span></a>
                            </p>
                        </div>
                    </div>
                </div>
            </section>
            <div className="text-center p-4 copyright">
                © 2023 Copyright:
                <a className="text-reset fw-bold ms-1" href="https://GameStore.gg/">GameStore.gg</a>
            </div>
        </footer>
    )
}

export default Footer;