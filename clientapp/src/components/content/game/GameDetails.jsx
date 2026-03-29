import React, { useEffect, useMemo, useRef, useState } from "react";
import { useSearchParams } from "react-router-dom";
import './game.css';
import iconDistribute from './../../../static/distribute.svg';
import iconRegion from './../../../static/key.svg';
import iconCopy from './../../../static/copy.svg';
import { GetUserData, subscribeOnGame } from "../../../utils/ApiRequests";
import { toast } from "react-toastify";
import { useTranslation } from 'react-i18next';
import { formatDate } from "../../../utils/i18nHelpers";
import { setItemsCount } from "../../NavMenu";
import Loading from "../../../utils/Loading";
import Price from "./parts/Price";
import ImageModal from "./parts/ImageModal";
import { POSTER } from "../../../utils/Constants";
import ModalSubscribe from "./parts/ModalSubscribe";

const GameDetails = ({ isAuthenticated, refreshAuth }) => {
    const [searchParams] = useSearchParams();
    const [game, setGame] = useState();
    const options = { year: 'numeric', month: 'long', day: 'numeric' };

    const modalSubscribe = useRef(null);

    const [isImageModal, setShowImageModal] = useState(false);
    const [currentImage, setCurrentImage] = useState();

    const { t } = useTranslation();

    const renderLoad = useMemo(() => {
        if (game === null) {
            return (
                <div className="absolute-centered">
                    <h2>{t('game.notFound')}</h2>
                </div>
            )
        }
        return (
            <Loading />
        )
    }, [game, t]);

    useEffect(() => {
        const requestInfo = "api/game/" + searchParams.get('id');
        const requestInit = {
            method: 'GET',
        };

        fetch(requestInfo, requestInit).then(response =>{
            if (!response.ok) {
                    setGame(null)
                    throw new Error(t('messages.errorOccurred'));
                }
            return response.json();
        }).then(result => {
            setGame(result)
        }).catch(err => toast.error(err));
    }, [searchParams])

    function addToCart() {
        var games = [];
        if (localStorage.games) {
            games = JSON.parse(localStorage.games);
        }

        if (games.some((element) => element.id === game.id)) {
            toast.info(t('game.alreadyInCart'))
            return;
        }

        games.push({
            id: game.id,
            count: 1,
        });
        setItemsCount(games.length);
        localStorage.games = JSON.stringify(games);
        toast.success(t('game.addedToCart'))
    }

    async function subscribe() {
        if (isAuthenticated) {
            const response = await GetUserData(refreshAuth);
            const user = await response.json();
            toast.promise(subscribeOnGame(game.id, user.email), {
                pending: t('subscribe.pending'),
                success: t('game.subscribeSuccess'),
                error: t('subscribe.error'),
            });
        }
        else {
            modalSubscribe.current.style.display = 'block';
        }
    }

    function showImageModal() {
        setCurrentImage(game.images.find(item => item.type.name === POSTER));
        setShowImageModal(true);
    }

    return (
        <div className="flex-container">
            {
                game && game.id ?
                    <div className="game-details d-flex my-3 p-3 overflow-hidden">
                        <div className="game-image">
                            <div style={{ maxWidth: '100%', width: '100%' }} className='overflow-hidden pointer' onClick={() => showImageModal()}>
                                <img src={game.images.find(item => item.type.name === POSTER).path} alt={searchParams.get('title')} className='width-inherit responsive-image' />
                            </div>
                                    <div className="description-grid bg-dark text-white py-2">
                                        <div>{t('game.labels.genre')}</div>
                                        <span>{game.genres.join(' ')}</span>
                                        <div>{t('game.labels.publisher')}</div>
                                        <span>{game.publisher}</span>
                                        <div>{t('game.labels.developer')}</div>
                                        <span>{game.developer}</span>
                                        <div>{t('game.labels.released')}</div>
                                        <span>{formatDate(game.released)}</span>
                                    </div>
                                    <button className="btn btn-outline-danger rounded-0 btn-responsive py-3 w-100" onClick={() => subscribe()}><span className="subscribe">{t('game.subscribe')}</span></button>
                        </div>
                        <div className="game-info">
                            <div className="d-flex flex-row flex-wrap-reverse gap-2 bg-dark justify-content-around text-white p-3 w-100">
                                <div>
                                    <h2 className="text-center">{game.title}</h2>
                                    <div className="game-type">
                                        <div>
                                            <div className="game-info-img">
                                                <img src={iconDistribute} alt="iconDistribute" />
                                            </div>
                                            <strong>{t('game.labels.platform')}: </strong><span className="text-capitalize">{game.platform}</span>
                                        </div>
                                        <div>
                                            <div className="game-info-img">
                                                <img src={iconRegion} alt="iconRegion" />
                                            </div>
                                            <strong>{t('game.labels.available')}: </strong><span>{game.regions.join(' ')}</span>
                                        </div>
                                        <div>
                                            <div className="game-info-img">
                                                <img src={iconCopy} alt="iconCopy" />
                                            </div>
                                            <strong>{t('game.labels.copyType')}: </strong><span className=" text-capitalize">{game.copyType}</span>
                                        </div>
                                        <div>
                                        </div>
                                    </div>
                                </div>

                                <div className="payment-info p-4 flex-fill">
                                    <Price item={game} discountClassName="fs-5" priceClassName="fs-1" vertical={true} />
                                    {
                                        game.copyCount !== 0 && game.isAvailable ?
                                            <div>
                                                <button className="btn btn-outline-success rounded-0 btn-responsive py-3 w-100" onClick={() => addToCart()}><span className="add">{t('game.addToCart')}</span></button>
                                            </div>
                                            :
                                            <button className="btn btn-outline-danger rounded-0 btn-responsive py-3 w-100" onClick={() => subscribe()}><span className="subscribe">{t('game.notifyWhenAvailable')}</span></button>
                                    }
                                </div>
                            </div>
                            <div className="game-description bg-dark text-white">
                                <h3>{t('game.description')}</h3>
                                <p dangerouslySetInnerHTML={game ? { __html: game.description } : { __html: '' }}></p>
                            </div>
                        </div>
                        <ImageModal images={game.images}
                            current={currentImage ?? game.images[0]}
                            setImageToShow={setCurrentImage}
                            showContent={isImageModal}
                            setShowContent={setShowImageModal} />
                    </div>
                    :
                    renderLoad
            }
            <ModalSubscribe refModal={modalSubscribe} game={game} />
        </div>
    )
}
export default GameDetails;