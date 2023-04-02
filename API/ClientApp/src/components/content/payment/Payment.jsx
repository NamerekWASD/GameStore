import React, { useEffect, useState } from "react";
import { paymentType } from "../game/ShoppingCart";
import BitCoin from "./BitCoin";
import { useNavigate } from "react-router-dom";
import { AppPaths } from "../../../utils/AppPaths";
import { toast } from "react-toastify";
import Loading from "../../../utils/Loading";
import Braintree from "./Braintree";

const Payment = () => {
    const navigate = useNavigate();
    const [sended, setSended] = useState(false);
    const sCartData = JSON.parse(localStorage.sCartData);

    const changeState = (value) => setSended(value);

    const onSuccess = async (billData, nonce) => {
        const requestInfo = `api/order/create`;
        const body = {
            userEmail: sCartData.userEmail,
            games: sCartData.games,
            BillingAddress: billData,
            nonce: nonce
        }
        console.log(billData);
        const requestInit = {
            method: 'POST',
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(body),
        };

        const promise = fetch(requestInfo, requestInit);

        const toastPromise = toast.promise(promise, {
            pending: "Ще трохи...",
        })
        const response = await promise;
        if (!response.ok) {
            toast.dismiss(toastPromise);
            toast.error(await response.text())
            setSended(false);
            return
        }
        toast.success("Готово!")
        var orderId = await response.json();
        setSended(false);
        navigate(AppPaths.orderDetails + '?' + new URLSearchParams([['orderId', orderId]]));
    }

    const onError = (errorMessage) => {
        toast(errorMessage);
    }

    function renderPayment() {
        switch (sCartData.payment) {
            case paymentType.card || paymentType.paypal:
                return (
                    <Braintree email={sCartData.userEmail}
                        setSended={changeState}
                        onSuccess={onSuccess}
                        onError={onError}
                        optionType={sCartData.payment}
                    />
                )
            case paymentType.crypto:
                return (
                    <BitCoin setSended={changeState} onSuccess={onSuccess} onError={onError} totalPriceUSD={sCartData.totalPrice} />
                )
            default:
                return (
                    <Braintree email={sCartData.userEmail}
                        setSended={changeState}
                        onSuccess={onSuccess}
                        onError={onError}
                        optionType={sCartData.payment}
                    />
                )
        }
    }

    return (
        <>
            {
                !sended ?
                    renderPayment()
                    :
                    <Loading />
            }
        </>
    )
}
export default Payment;