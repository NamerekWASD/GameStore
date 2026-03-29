import { useState } from "react";
import { paymentType } from "../game/ShoppingCart";
import BitCoin from "./BitCoin";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import Loading from "../../../utils/Loading";
import Braintree from "./Braintree";
import { setItemsCount } from "../../NavMenu";
import { AppPaths } from "../../../utils/AppPaths";

const Payment = () => {
    const [sended, setSended] = useState(false);
    const sCartData = JSON.parse(localStorage.sCartData);
    const navigate = useNavigate();

    const changeState = (value) => setSended(value);

    const onError = (errorMessage) => {
        toast(errorMessage);
    }

    const onSuccess = async (result) => {
        setSended(false);
        localStorage.games = [];
        setItemsCount(0);
        navigate(AppPaths.orderDetails + '?' + new URLSearchParams([['orderNumber', result]]));
    }

    function renderPayment() {
        switch (sCartData.payment) {
            case paymentType.card || paymentType.paypal:
                return (
                    <Braintree 
                        sCartData={sCartData}
                        onSuccess={onSuccess} 
                        setSended={changeState}
                        onError={onError}
                        optionType={sCartData.payment}
                    />
                )
            case paymentType.crypto:
                return (
                    <BitCoin setSended={changeState} onError={onError} totalPriceUSD={sCartData.totalPrice} />
                )
            default:
                return (
                    <Braintree 
                        sCartData={sCartData}
                        onSuccess={onSuccess} 
                        setSended={changeState}
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