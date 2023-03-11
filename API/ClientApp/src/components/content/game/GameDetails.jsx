import React, { useEffect, useRef, useState } from "react";
import { useSearchParams } from "react-router-dom";
import './game.css';
import iconDistribute from './../../../static/distribute.svg';
import iconRegion from './../../../static/key.svg';
import iconCopy from './../../../static/copy.svg';

const GameDetails = () => {
    const [searchParams] = useSearchParams();
    const [game, setGame] = useState({});

    const gameImage = useRef(null);
    const gameTitle = useRef(null);
    const gamePlatform = useRef(null);
    const gameCopyType = useRef(null);
    const gamePrice = useRef(null);
    const description = useRef(null);
    const gameRegionAvailable = useRef(null);
    const gameGenre = useRef(null);
    const gamePublisher = useRef(null);
    const gameDeveloper = useRef(null);
    const gameReleased = useRef(null);



    async function loadGame() {
        const requestInfo = "api/game/" + searchParams.get('id');
        const requestInit = {
            method: 'GET',
        };

        const response = await fetch(requestInfo, requestInit)
        const responseBody = await response.json();
        return responseBody;
    }
    useEffect(() => {
        if (!game.id) {
            loadGame()
                .then(result => {
                    setGame(result)
                });
        } else {
            renderGame();
        }
    }, [game, renderGame])

    function renderGame() {
        gameImage.current.src = game.imageURL;
        gameTitle.current.textContent = game.title;
        gamePlatform.current.textContent = game.platform;
        gameCopyType.current.textContent = game.copyType;
        gamePrice.current.textContent = game.price;
        description.current.textContent = game.description;
        gameRegionAvailable.current.textContent = game.regions.join(', ');
        gameGenre.current.textContent = game.genres.join(' ');
        gamePublisher.current.textContent = game.publisher;
        gameDeveloper.current.textContent = game.developer;
        const event = new Date(Date.parse(game.released));
        const options = {year: 'numeric', month: 'long', day: 'numeric' };
        gameReleased.current.textContent = event.toLocaleDateString('uk-UA', options);
    }

    function addToCart() {
        var games = [];
        if (localStorage.games) {
            games = JSON.parse(localStorage.games);
        }
        var counter = document.getElementById("shopping-cart-counter");

        if (games.some((element) => element.id == game.id)) {
            return
        }

        games.push(game);
        counter.textContent = parseInt(counter.textContent) + 1;

        localStorage.games = JSON.stringify(games);
    }

    return (
        <div className="flex-container">
            <div className="game-details d-flex my-3 p-3 overflow-hidden">
                <div className="game-image">
                    <img ref={gameImage} alt={searchParams.get('title')} />
                    <div className="description-grid bg-dark text-white py-2">
                        <div>Жанр</div>
                        <span ref={gameGenre}></span>
                        <div>Видавець</div>
                        <span ref={gamePublisher}></span>
                        <div>Розробник</div>
                        <span ref={gameDeveloper}></span>
                        <div>Дата випуску</div>
                        <span ref={gameReleased}></span>
                    </div>
                </div>
                <div className="game-info">
                    <div className="d-flex flex-row flex-wrap-reverse gap-2 bg-dark justify-content-around text-white p-3 w-100">
                        <div>
                            <h2 className="text-center" ref={gameTitle}></h2>
                            <div className="game-type">
                                <div>
                                    <div className="game-info-img">
                                        <img src={iconDistribute} />
                                    </div>
                                    <strong>Платформа: </strong><span className=" text-capitalize" ref={gamePlatform}></span>
                                </div>
                                <div>
                                    <div className="game-info-img">
                                        <img src={iconRegion} />
                                    </div>
                                    <strong>Доступно в: </strong><span ref={gameRegionAvailable}></span>
                                </div>
                                <div>
                                    <div className="game-info-img">
                                        <img src={iconCopy} />
                                    </div>
                                    <strong>Тип копії: </strong><span className=" text-capitalize" ref={gameCopyType}></span>
                                </div>
                                <div>
                                </div>
                            </div>
                        </div>

                        <div className="payment p-4 bg-light">
                            <div className="product-price"><span ref={gamePrice}></span> <sup>$</sup></div>
                            <button className="btn btn-outline-success add-to-basket py-3" onClick={() => addToCart()}><span>Додати у кошик</span></button>
                        </div>
                    </div>
                    <div className="game-description bg-dark text-white">
                        <h3>Опис</h3>
                        <p ref={description}></p>
                    </div>
                </div>
            </div>
        </div>
    )
}
export default GameDetails;