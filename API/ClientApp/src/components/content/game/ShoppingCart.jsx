import React, { createRef, useEffect, useMemo, useRef, useState } from "react";
import './game.css'
import minus from './../../../static/minus.svg';
import plus from './../../../static/plus.svg';
import remove from './../../../static/close-cross.svg'
import { useNavigate } from "react-router-dom";
import { AppPaths } from "../../../utils/AppPaths";
import { loadUserData, SendLoginData, subscribeOnGame } from "../../../utils/ApiRequests";
import EmailConfirmation from "../user/Auth/EmailConfirmation";
import { toast } from "react-toastify";
import { navigateToDetails } from "../../../utils/Navigation";
import { setItemsCount } from "../../NavMenu";
import Price from "./parts/Price";
import ModalSubscribe from "./parts/ModalSubscribe";

export const paymentType = {
    card: 'Visa/Mastercard',
    paypal: 'PayPal',
    crypto: 'BitCoin'
}

const ShoppingCart = ({ isAuthenticated, refreshAuth }) => {
    const navigate = useNavigate();

    const [games, setGames] = useState([]);
    const [payment, setPayment] = useState("Visa/Mastercard");
    const [gameToSubscibe, setGameToSubscibe] = useState();
    const total = useRef(null);
    const email = useRef(null);

    const modal = useRef(null);
    const modalConfirm = useRef(null);
    const modalSubscribe = useRef(null);

    const countRefs = useRef([]);
    countRefs.current = games.map((_, index) => countRefs.current[index] ?? createRef());

    const memoRenderGames = useMemo(() => renderGames(), [games])

    useEffect(() => {
        if (localStorage.games && localStorage.games.length !== 0) {
            loadSpecifiedtGames(JSON.parse(localStorage.games)).then(result => setGames(result));
        }
    }, [])

    useEffect(() => {
        if (games.length !== 0) {
            localStorage.games = JSON.stringify(games);
            recalculateTotalPrice();
        }
    }, [games])

    useEffect(() => {
        loadUserData(false, navigate)
            .then(response => response.json())
            .then(result => {
                if (result.email)
                    email.current.value = result.email;
            })
    }, [isAuthenticated])

    async function loadSpecifiedtGames(storageGames) {
        let games = [];
        storageGames.forEach(element => {
            games.push(parseInt(element.id));
        });

        const requestInfo = "api/game/list";
        const requestInit = {
            method: 'POST',
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(games),
        };
        const response = await (await fetch(requestInfo, requestInit)).json();
        games = [];
        storageGames.forEach(element => {
            var game = response.find((object) => object.id === element.id);
            game.count = element.count ?? 1;
            games.push(game);
        });
        return games;
    }

    function renderGames() {
        return (
            games.length !== 0 ? games.map((game, index) => {
                return (
                    <div key={game.id} className="mb-3 my-card bg-white">
                        <div className="overflow-hidden" style={{ width: '150px' }}>
                            <img src={game.image.path} alt={game.title} className="pointer width-inherit" onClick={() => navigateToDetails(game, navigate)} />
                        </div>
                        <div className="card-body flex-fill d-flex flex-row flex-wrap justify-content-center">
                            <div className="flex-fill my-auto px-3 pointer" onClick={() => navigateToDetails(game, navigate)}>
                                <h5 className="card-title text-center">{game.title}</h5>
                            </div>

                            {
                                game.copyCount !== 0 ?
                                    <div className="d-flex flex-row gap-4">
                                        <Price item={game} />
                                        <div className="text-center my-auto border-bottom border-success border-2">
                                            <div className="d-flex flex-row align-items-center px-2">
                                                <button className="p-0 btn btn-crement text-center"
                                                    onClick={() => changeCopiesCount(game, index, -1)}>
                                                    <img src={minus} alt="minus" />
                                                </button>
                                                <input ref={countRefs.current[index]} className="count-input fs-4 text-center border-0 bg-white" type='text'
                                                    value={game.count} min="1" max={game.copyCount <= 5 ? game.copyCount : 5} disabled></input>
                                                <button className="p-0 btn btn-crement text-center"
                                                    onClick={() => changeCopiesCount(game, index, 1)}>
                                                    <img src={plus} alt="plus" />
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                    :
                                    <div className="text-center my-auto">
                                        <h5>Нажаль копій цієї гри не залишилось</h5>
                                        <button className="btn btn-outline-dark responsive-btn rounded-0" onClick={() => subscribe(game)}>Повідомити мене коли з'явиться гра</button>
                                    </div>
                            }
                        </div>
                        <img className="pe-2 pointer ms-2" src={remove} onClick={() => removeGameFromShoppingBasket(game.id)} alt="remove" />
                    </div>

                )
            }
            )
                :
                <>
                    <h4 className="text-muted">Ви ще нічого не додали до кошику!</h4>
                    <button className="btn btn-outline-success rounded-0 w-100" onClick={() => navigate(AppPaths.gameCatalog)}>Продовжити покупки</button>
                </>

        )
    }

    function subscribe(game) {
        setGameToSubscibe(game)
        if (isAuthenticated) {
            subscribeOnGame(game.id, email.current.value);
        }
        else {
            modalSubscribe.current.style.display = 'block';
        }
        removeGameFromShoppingBasket(game.id);
    }

    function removeGameFromShoppingBasket(id) {
        const newList = games.filter((item) => item.id !== id).map(item => {
            return {id: item.id, count: item.count}
        });
        localStorage.games = JSON.stringify(newList);
        setGames(prevState => prevState.filter(item => newList.some(game => game.id === item.id)));
        setItemsCount(newList.length);
    }

    function changeCopiesCount(game, index, input) {
        const element = countRefs.current[index];
        var value = parseInt(element.current.value) + input;
        value = isNaN(value) ? 1 : value;
        if (value > element.current.max || value < 1) return;
        element.current.value = value;
        games.find(item => item.id === game.id).count = value;
        recalculateTotalPrice()
    }

    function recalculateTotalPrice() {
        let sum = 0;
        games.forEach(game => {
            sum += (game.discountPrice ?? game.price) * game.count
        });
        total.current.textContent = +sum.toFixed(2);
    }

    async function submitForm(e) {
        e.preventDefault();

        if (games.length === 0) {
            modal.current.style.display = "block";
            window.onclick = function (event) {
                if (event.target == modal) {
                    modal.current.style.display = "none";
                }
            }
            return;
        }

        if (!isAuthenticated) {
            await sendConfirmation()
            modalConfirm.current.style.display = 'block';
            return;
        }
        boxData();
    }

    function boxData() {
        const gamesModel = [];
        games.forEach((game, index) => {
            if (game.copyCount !== 0) {
                const newGameModel = {
                    id: game.id,
                    count: game.count,
                    price: game.discountPrice ?? game.price,
                }
                gamesModel[index] = newGameModel;
            }
        });

        const bill = {
            userEmail: email.current.value,
            games: gamesModel,
            payment: payment,
        }

        localStorage.sCartData = JSON.stringify(bill);
        navigate(AppPaths.payment)
    }

    const sendConfirmation = async () => {
        const LoginModel = {
            email: email.current.value,
        };

        const response = await SendLoginData(LoginModel);

        if (processResponse(response, true)) {
            modalConfirm.current.style.display = "block";
        }
    }
    const processResponse = async (response) => {
        if (!response.ok) {
            toast.error(await response.text());
            return false;
        }
        return true;
    }
    const emailConfirmationAfter = async (response) => {
        if (!response.ok) {
            toast.error(await response.text());
            return false;
        }
        toast.success("Пошту підтвердженно");
        refreshAuth();
        modalConfirm.current.style.display = 'none'
        boxData();
    }

    return (
        <div className="d-flex flex-wrap-reverse gap-3 shopping-cart-container" style={{ maxWidth: "1500px" }}>
            <div className="card-container col mx-auto">
                <h3><b>Ваш кошик</b></h3>
                {memoRenderGames}
                {
                    games.length !== 0 ?
                        <>
                            <hr />
                            <h2 className="text-end"><span>Сума: </span><span ref={total}></span><sup>$</sup></h2>
                        </>
                        : ''
                }
            </div>
            <div className="order-container">
                <h3><b>Оформлення замовлення</b></h3>
                <div className="order-form bg-white">
                    <form onSubmit={(e) => submitForm(e)}>
                        <div className="form-group required">
                            <label htmlFor="email" className="control-label">Електронна пошта</label>
                            <input ref={email} id="email" className="form-control rounded-0" type="email"
                                placeholder="Електронна пошта" onChange={(e) => e.target.value} required />
                        </div>
                        <fieldset className="form-group required">
                            <div className="row">
                                <legend className="col-form-label col-sm-2 pt-0">Оплата</legend>
                                <div className="col-sm-10">
                                    <div className="form-check">
                                        <input className="form-check-input" type="radio" name="payment" id="gridRadios1"
                                            value={paymentType.card} defaultChecked onChange={(e) => setPayment(e.target.value)} />
                                        <label className="form-check-label" htmlFor="gridRadios1">
                                            Visa/Mastercard
                                        </label>
                                    </div>
                                    <div className="form-check">
                                        <input className="form-check-input" type="radio" name="payment" id="gridRadios3"
                                            value={paymentType.crypto} onChange={(e) => setPayment(e.target.value)} />
                                        <label className="form-check-label" htmlFor="gridRadios3">
                                            BitCoin
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                        <div className="form-group checkbox-unique">
                            <input id="callback" type="checkbox" name="callback" required />
                            <label className="form-check-label" htmlFor="callback">Я впевненений у коректності даних</label>
                        </div>
                        <button type="submit" className="btn btn-primary w-100 rounded-0">Підтвердити</button>
                    </form>
                </div>
            </div>
            <div ref={modal} className="modal">
                <div className="modal-content rounded-0">
                    <div className="modal-header rounded-0 d-block p-3">
                        <h3 className="text-center">У вашому кошику жодної гри!
                            <span id="close-modal" className="close-modal float-end" onClick={() => modal.current.style.display = 'none'}>&times;</span></h3>
                    </div>
                    <div className="modal-body p-2 text-center m-3">
                        <button className="btn btn-outline-success rounded-0 w-100" onClick={() => navigate(AppPaths.gameCatalog)}>Продовжити покупки</button>
                    </div>
                </div>
            </div>
            <EmailConfirmation refModal={modalConfirm} refEmail={email} after={emailConfirmationAfter} />
            <ModalSubscribe refModal={modalSubscribe} game={gameToSubscibe} />
        </div>
    )
}

export default ShoppingCart;