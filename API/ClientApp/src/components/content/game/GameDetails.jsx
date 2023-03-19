import React, { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { useSearchParams } from "react-router-dom";
import './game.css';
import iconDistribute from './../../../static/distribute.svg';
import iconRegion from './../../../static/key.svg';
import iconCopy from './../../../static/copy.svg';
import ModalSubscribe from "../../../utils/ModalSubscribe";
import { loadUserData, subscribeOnGame } from "../../../utils/ApiRequests";
import { toast } from "react-toastify";
import { setItemsCount } from "../../NavMenu";
import Loading from "../../../utils/Loading";
import Price from "./parts/Price";

const GameDetails = ({ isAuthenticated }) => {
    const [searchParams] = useSearchParams();
    const [game, setGame] = useState({});
    const options = { year: 'numeric', month: 'long', day: 'numeric' };

    const modalSubscribe = useRef(null);



    async function loadGame() {
        const requestInfo = "api/game/" + searchParams.get('id');
        const requestInit = {
            method: 'GET',
        };

        const response = await fetch(requestInfo, requestInit)
        if (!response.ok) {
            setGame(null)
            return;
        }
        const responseBody = await response.json();
        return responseBody;
    }

    const renderLoad = useMemo(() => {
        if (!game) {
            return (
                <div className="absolute-centered">
                    <h2>Гру не знайдено</h2>
                </div>
            )
        }
        return (
            <Loading />
        )
    }, [game]);

    useEffect(() => {
        loadGame()
            .then(result => {
                setGame(result)
            });
    }, [searchParams])

    function addToCart() {
        var games = [];
        if (localStorage.games) {
            games = JSON.parse(localStorage.games);
        }

        if (games.some((element) => element.id == game.id)) {
            toast.info("Ви вже додали цю гру в кошик")
            return;
        }

        games.push(game);
        setItemsCount(games.length);
        localStorage.games = JSON.stringify(games);
        toast.success("Гра успішно додана!")
    }

    async function subscribe() {
        if (isAuthenticated) {
            const response = await loadUserData();
            const user = await response.json();
            toast.promise(subscribeOnGame(game.id, user.email), {
                pending: 'Зачекайте...',
                success: 'Ви успішно підписалися на гру!\n Очікуйте нових повідомлень.',
                error: 'Нажаль, сталася помилка',
            });
        }
        else {
            modalSubscribe.current.style.display = 'block';
            window.addEventListener('click', () => {
                modalSubscribe.current.style.display = 'none';
            })
        }
    }

    return (
        <div className="flex-container">
            {
                game && game.id ?
                    <div className="game-details d-flex my-3 p-3 overflow-hidden">
                        <div className="game-image">
                            <div style={{ maxWidth: '100%', width: '100%' }} className='overflow-hidden'>
                                <img src={game.images.find(item => item.name === 'portrait').path} alt={searchParams.get('title')} className='width-inherit' />
                            </div>
                            <div className="description-grid bg-dark text-white py-2">
                                <div>Жанр</div>
                                <span>{game.genres.join(' ')}</span>
                                <div>Видавець</div>
                                <span>{game.publisher}</span>
                                <div>Розробник</div>
                                <span>{game.developer}</span>
                                <div>Дата випуску</div>
                                <span>{new Date(Date.parse(game.released)).toLocaleDateString('uk-UA', options)}</span>
                            </div>
                        </div>
                        <div className="game-info">
                            <div className="d-flex flex-row flex-wrap-reverse gap-2 bg-dark justify-content-around text-white p-3 w-100">
                                <div>
                                    <h2 className="text-center">{game.title}</h2>
                                    <div className="game-type">
                                        <div>
                                            <div className="game-info-img">
                                                <img src={iconDistribute} />
                                            </div>
                                            <strong>Платформа: </strong><span className="text-capitalize">{game.platform}</span>
                                        </div>
                                        <div>
                                            <div className="game-info-img">
                                                <img src={iconRegion} />
                                            </div>
                                            <strong>Доступно в: </strong><span>{game.regions.join(' ')}</span>
                                        </div>
                                        <div>
                                            <div className="game-info-img">
                                                <img src={iconCopy} />
                                            </div>
                                            <strong>Тип копії: </strong><span className=" text-capitalize">{game.copyType}</span>
                                        </div>
                                        <div>
                                        </div>
                                    </div>
                                </div>

                                <div className="payment-info p-4 flex-fill">
                                        <Price item={game} discountClassName="fs-5" priceClassName="fs-1" vertical={true}/>
                                    {
                                        game.copyCount !== 0 ?
                                            <button className="btn btn-outline-success rounded-0 btn-responsive py-3 w-100" onClick={() => addToCart()}><span className="add">Додати у кошик</span></button>
                                            :
                                            <button className="btn btn-outline-danger rounded-0 btn-responsive py-3 w-100" onClick={() => subscribe()}><span className="subscribe">Повідомити коли з'явиться</span></button>
                                    }
                                </div>
                            </div>
                            <div className="game-description bg-dark text-white">
                                <h3>Опис</h3>
                                <p>{game.description}</p>
                            </div>
                        </div>
                    </div>
                    :
                    renderLoad
            }
            <ModalSubscribe refModal={modalSubscribe} game={game} />
        </div>
    )
}
export default GameDetails;