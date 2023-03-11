import React, { useState } from "react";
import { paymentType } from "../game/ShoppingCart";
import PayPal from "./PayPal";
import CreditCard from "./CreditCard";
import BitCoin from "./BitCoin";
import { useNavigate } from "react-router-dom";
import { AppPaths } from "../../../utils/AppPaths";

const Payment = () => {
    const navigate = useNavigate();
    const [sended, setSended] = useState(false);

    const changeState = (value) => setSended(value);
    const onSuccess = (value) => {
        const body = {
            userEmail: sCartData.userEmail,
            password: sCartData.password,
            games: sCartData.games,
            billingAddress: value,
        }
        localStorage.sCartData = JSON.stringify(body);
        

        navigate(AppPaths.paymentSuccess)
    };
    const sCartData = JSON.parse(localStorage.sCartData);
    function renderPayment() {
        switch (sCartData.payment) {
            case paymentType.card:
                return (
                    <>
                        <CreditCard email={sCartData.userEmail} setSended={changeState} onSuccess={onSuccess} />
                    </>
                )
            case paymentType.paypal:
                return (
                    <>
                        <PayPal />
                    </>
                )
            case paymentType.crypto:
                return (
                    <>
                        <BitCoin />
                    </>
                )
            default:
                return (
                    <>
                        <CreditCard />
                    </>
                )
        }
    }

    return (
        <>
            {
                !sended ?
                    renderPayment()
                    :
                    <div className="absolute-centered">
                        <div className="waviy">
                            <span className="i1">G</span>
                            <span className="i2">G</span>
                        </div>
                    </div>
            }
        </>
    )
}
export default Payment;