import React, { useEffect, useState } from "react";
const PaymentSuccess = () => {
    const [requested, setRequested] = useState(false);
    useEffect(() => {
        if (!requested) {
            setRequested(true);

            makeRequest();
        }
    })
    async function makeRequest() {
        const sCartData = JSON.parse(localStorage.sCartData)
        const requestInfo = `api/cheque/create`;
        const body = {
            userEmail: sCartData.userEmail,
            password: sCartData.password,
            games: sCartData.games,
            billingAddress: sCartData.billingAddress,
        }
        const requestInit = {
            method: 'POST',
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(body),
        };

        const response = await fetch(requestInfo, requestInit).catch(err => console.log(err));

        if (!response.ok) {
            console.log(await response.text());
            return
        }
        console.log(await response.json());
    }

    return (
        <>

        </>
    )
}
export default PaymentSuccess;