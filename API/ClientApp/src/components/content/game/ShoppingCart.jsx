import React, { createRef, useEffect, useMemo, useRef, useState } from "react";
import './game.css'
import minus from './../../../static/minus.svg';
import plus from './../../../static/plus.svg';
import remove from './../../../static/close-cross.svg'
import { useNavigate } from "react-router-dom";
import { AppPaths } from "../../../utils/AppPaths";
import { loadUserData } from "../../../utils/ApiRequests";


export const paymentType = {
    card: 'Visa/Mastercard',
    paypal: 'PayPal',
    crypto: 'BitCoin'
}

const ShoppingCart = () => {
    const navigate = useNavigate();

    const [games, setGames] = useState([]);
    const total = useRef(null);

    const email = useRef(null);
    const [payment, setPayment] = useState("Visa/Mastercard");
    const AdditionInfo = useRef(null);


    const countRefs = useRef([]);
    countRefs.current = games.map((_, index) => countRefs.current[index] ?? createRef());
    const priceRefs = useRef([]);
    priceRefs.current = games.map((_, index) => priceRefs.current[index] ?? createRef());

    const renderGames = useMemo(() => renderCards(), [games])

    useEffect(() => {
        if (localStorage.games && localStorage.games.length !== 0 && games.length === 0) {
            loadSpecifiedtGames(JSON.parse(localStorage.games)).then(result => setGames(result));
        }
        if (games.length !== 0) {
            localStorage.games = JSON.stringify(games);
            recalculateValue();
        } else {
            localStorage.games = [];
        }
    })
    useEffect(() => {
        loadUserData(navigate, false, true).then(result => {
            if(result.email)
                email.current.value = result.email;
        })
    })

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
    function renderCards() {
        return (
            games.length !== 0 ? games.map((game, index) => {
                return (
                    <div key={game.id} className="mb-3 my-card bg-white">
                        <img src={game.imageURL} style={{ width: '100px', height: '130px' }} alt={game.title} />
                        <div className="card-body flex-fill d-flex flex-row gap-4">
                            <div className="flex-fill my-auto px-3">
                                <h5 className="card-title text-center">{game.title}</h5>
                            </div>

                            <div className="my-auto">
                                <div className="text-center">
                                    <div className="border border-2 d-flex flex-row align-items-center">
                                        <button className="p-0 btn btn-crement text-center"
                                            onClick={() => changeValue(game.id, index, -1)}>
                                            <img src={minus} alt="minus" />
                                        </button>
                                        <input ref={countRefs.current[index]} className="count-input fs-4 text-center border-end border-start bg-transparent p-0 " type='text'
                                            value={game.count} min="1" max={game.copyCount} disabled></input>
                                        <button className="p-0 btn btn-crement text-center"
                                            onClick={() => changeValue(game.id, index, 1)}>
                                            <img src={plus} alt="plus" />
                                        </button>
                                    </div>
                                </div>
                            </div>
                            <div className="card-text text-xxl-center fs-5 fw-bold text-center my-auto">
                                <span ref={priceRefs.current[index]} className="game-price">{+(game.price * game.count).toFixed(2)}</span><sup>$</sup>
                            </div>
                            <img className="my-auto pe-2 pointer" src={remove} onClick={() => removeItem(game.id)} alt="cross" />
                        </div>
                    </div>

                )
            }
            )
                :
                <h4 className="text-muted">Ви ще нічого не додали до кошику!</h4>

        )
    }

    function removeItem(id) {
        const newList = games.filter((item) => item.id !== id);
        localStorage.games = JSON.stringify(newList);
        setGames(newList);
        document.getElementById('shopping-cart-counter').textContent = newList.length;
    }

    function changeValue(id, index, input) {
        const element = countRefs.current[index];
        var value = parseInt(element.current.value) + input;
        value = isNaN(value) ? 1 : value;
        if (value > element.current.max || value < 1) return;
        element.current.value = value;
        recalculateValue(id, element.current.value, index);
    }

    function recalculateValue(id, count, index) {
        games.some((game) => {
            if (game.id === id) {
                game.count = count;
                priceRefs.current[index].current.textContent = +(game.price * count).toFixed(2);
                return true;
            }
            return false;
        });
        let sum = 0;
        var prices = document.getElementsByClassName("game-price")
        for (let index = 0; index < prices.length; index++) {
            const element = prices[index];
            sum += +parseFloat(element.textContent);
        };
        total.current.textContent = +sum.toFixed(2);
    }

    const handlePayment = (e) => {
        setPayment(e.target.value);
    }

    function submit(e) {
        e.preventDefault();
        if (games.length === 0) {
            var modal = document.getElementById("myModal");
            var span = document.getElementById("close-modal");
            modal.style.display = "block";
            span.onclick = function () {
                modal.style.display = "none";
            }
            window.onclick = function (event) {
                if (event.target == modal) {
                    modal.style.display = "none";
                }
            }
            return;
        }
        const gamesModel = [];
        games.forEach((game, index) => {
            const newGameModel = {
                id: game.id,
                count: game.count,
                price: game.price,
            }
            gamesModel[index] = newGameModel;
        });
        const bill = {
            userEmail: email.current.value,
            games: gamesModel,
            payment: payment,
            additionInfo: AdditionInfo.current.value,
        }
        const password = document.getElementById('password');
        if(password){
            bill.password = password.value;
        }
        localStorage.sCartData = JSON.stringify(bill);
        navigate(AppPaths.payment)
    }
    return (
        <div className="d-flex flex-wrap-reverse gap-3 shopping-cart-container">
            <div className="card-container col mx-auto">
                <h3><b>Ваш кошик</b></h3>
                {renderGames}
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
                    <form onSubmit={(e) => submit(e)}>
                        <div className="form-group required">
                            <label htmlFor="email" className="control-label">Електронна пошта</label>
                            <input ref={email} id="email" className="form-control rounded-0" type="email" placeholder="Електронна пошта" required />
                        </div>
                        <fieldset className="form-group required">
                            <div className="row">
                                <legend className="col-form-label col-sm-2 pt-0">Оплата</legend>
                                <div className="col-sm-10">
                                    <div className="form-check">
                                        <input className="form-check-input" type="radio" name="payment" id="gridRadios1"
                                            value={paymentType.card} defaultChecked onChange={handlePayment} />
                                        <label className="form-check-label" htmlFor="gridRadios1">
                                            Visa/Mastercard
                                        </label>
                                    </div>
                                    <div className="form-check">
                                        <input className="form-check-input" type="radio" name="payment" id="gridRadios2"
                                            value={paymentType.paypal} onChange={handlePayment} />
                                        <label className="form-check-label" htmlFor="gridRadios2">
                                            PayPal
                                        </label>
                                    </div>
                                    <div className="form-check">
                                        <input className="form-check-input" type="radio" name="payment" id="gridRadios3"
                                            value={payment.crypto} onChange={handlePayment} />
                                        <label className="form-check-label" htmlFor="gridRadios3">
                                            BitCoin
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                        <div className="form-group">
                            <input className="form-check-input" id="callback" type="checkbox" name="callback" required />
                            <label className="form-check-label ms-2 rounded-0" htmlFor="callback">Я впевненений у коректності даних</label>
                        </div>
                        <div className="form-group">
                            <textarea ref={AdditionInfo} className="form-control rounded-0" id="additional-information" rows="3"></textarea>
                            <label htmlFor="additional-information" className="form-label">Додаткова інформація</label>
                        </div>
                        <button type="submit" className="btn btn-primary w-100 rounded-0">Підтвердити</button>
                    </form>
                </div>
            </div>
            <div id="myModal" className="modal">
                <div className="modal-content rounded-0">
                    <div className="modal-header rounded-0 d-block p-3">
                        <h3 className="text-center">У вашому кошику жодної гри!
                            <span id="close-modal" className="close-modal float-end">&times;</span></h3>
                    </div>
                    <div className="modal-body p-2 text-center m-3">
                        <button className="btn btn-outline-success rounded-0 w-100" onClick={() => navigate(AppPaths.gameSearch)}>Продовжити покупки</button>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default ShoppingCart;