import React, { useRef } from "react";
const CreditCard = ({ email, setSended, onSuccess }) => {
    const fullname = useRef(null);
    const emailRef = useRef(null);
    const address = useRef(null);
    const city = useRef(null);
    const region = useRef(null);
    const zip = useRef(null);

    const cardName = useRef(null);
    const cardNumber = useRef(null);
    const expirationMonth = useRef(null);
    const expirationYear = useRef(null);
    const CVV = useRef(null);

    async function submit(e){
        e.preventDefault();
        setSended(true)

        const paymentData = {
            cardName: cardName.current.value,
            cardNumber: cardNumber.current.value,
            expirationMonth: expirationMonth.current.value,
            expirationYear: expirationYear.current.value,
            CVV: CVV.current.value,
        } ;

        const billData = {
            fullname: fullname.current.value,
            email: emailRef.current.value,
            address: address.current.value,
            city: city.current.value,
            region: region.current.value,
            zip: zip.current.value,
        };

        // sending to payment system
        const success = await sendData(paymentData);
        if(success){
            onSuccess(billData)
        }
        setSended(false);
    }

    async function sendData(paymentData){
        // Doing staff
        return new Promise((resolve) => {
            setTimeout(resolve, 5*1000, true);
        });
    }
    return (
        <div className="bg-gray p-3 m-5">
            <form onSubmit={(e) => submit(e)}>
                <div className="d-flex gap-3 flex-wrap">
                    <div className="flex-fill bg-white p-3">
                        <h3>Платіжна адреса</h3>
                        <div className="form-group rounded-0">
                            <label className="control-label" htmlFor="fname"><i className="fa fa-user"></i>ПІБ</label>
                            <input ref={fullname} className="form-control rounded-0" type="text" id="fname" name="fullname" required />
                        </div>
                        <div className="form-group rounded-0">
                            <label className="control-label" htmlFor="email"><i className="fa fa-envelope"></i> Email</label>
                            <input ref={emailRef} className="form-control rounded-0" type="email" id="email" name="email" defaultValue={email ?? ''} required/>
                        </div>
                        <div className="form-group rounded-0">
                            <label className="control-label" htmlFor="adr"><i className="fa fa-address-card-o"></i>Адреса</label>
                            <input ref={address} className="form-control rounded-0" type="text" id="adr" name="address" required />
                        </div>
                        <div className="form-group rounded-0">
                            <label className="control-label" htmlFor="city"><i className="fa fa-institution"></i>Місто</label>
                            <input ref={city} className="form-control rounded-0" type="text" id="city" name="city" required />
                        </div>

                        <div className="d-flex justify-content-between">
                            <div className="form-group rounded-0">
                                <label className="control-label" htmlFor="region">Область</label>
                                <input ref={region} className="form-control rounded-0" type="text" id="region" name="region" required />
                            </div>
                            <div className="form-group rounded-0">
                                <label className="control-label" htmlFor="zip">Індекс</label>
                                <input ref={zip} className="form-control rounded-0" type="text" id="zip" name="zip" required />
                            </div>
                        </div>
                    </div>

                    <div className="flex-fill bg-white p-3">
                        <h3>Оплата</h3>
                        <div className="form-group rounded-0">
                            <label className="control-label" htmlFor="cname">Ім'я на карті</label>
                            <input ref={cardName} className="form-control rounded-0" type="text" id="cname" name="cardname" required />
                        </div>
                        <div className="form-group rounded-0">
                            <label className="control-label" htmlFor="ccnum">Номер картки</label>
                            <input ref={cardNumber} className="form-control rounded-0" type="password" id="ccnum" name="cardnumber" pattern="^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14})$" required />
                        </div>

                        <div className="d-flex justify-content-between">
                            <div className="form-group rounded-0">
                                <span className="control-label">Термін придатності</span>
                                <div className="d-flex gap-3 w-75">
                                    <input ref={expirationMonth} className="form-control rounded-0 w-50" type="number" id="expmonth" name="expmonth" min={1} max={12} placeholder="12" required maxLength={2} />
                                    <input ref={expirationYear} className="form-control rounded-0" type="number" id="expyear" name="expyear" min={new Date().getFullYear()} placeholder="2108" required maxLength={4} />
                                </div>
                            </div>
                            <div className="form-group rounded-0">
                                <label className="control-label" htmlFor="cvv">CVV</label>
                                <input ref={CVV} className="form-control rounded-0" type="text" id="cvv" name="cvv" placeholder="***" maxLength={3} required />
                            </div>
                        </div>
                    </div>
                </div>
                <div className="mt-4">
                    <button type="submit" className="btn btn-success rounded-0 w-100">Продовжити</button>
                </div>
            </form>
        </div>
    )
}
export default CreditCard;