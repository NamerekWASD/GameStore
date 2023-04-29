import React, { useEffect, useMemo, useRef, useState } from "react";
import { Button } from "reactstrap";
import dropin from "braintree-web-drop-in"
import { GetLastBill } from "../../../utils/ApiRequests";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faGlobe, faHome, faUser } from "@fortawesome/free-solid-svg-icons";
import countryList from 'react-select-country-list'
import Select from 'react-select'

const Braintree = ({ email, setSended, onSuccess, onError }) => {
    const [braintreeInstance, setBraintreeInstance] = useState(undefined)

    const firstName = useRef(null);
    const lastName = useRef(null);
    const streetAddress = useRef(null);
    const region = useRef(null);
    const postalCode = useRef(null);
    const billingAddressId = useRef(null);
    const [country, setCountry] = useState({});
    const countries = useMemo(() => countryList().getData(), [])

    const changeHandler = (value) => {
        setCountry(value);
    }

    const initializeBraintree = () => dropin.create({
        authorization: process.env.REACT_APP_BRAINTREE_AUTHORIZATION_CODE,
        container: '#braintree-drop-in-div',
        preselectVaultedPaymentMethod: true,
    }, function (error, instance) {
        if (error)
            console.error(error)
        else {
            setBraintreeInstance(instance);
        }
    });

    useEffect(() => {
        if (braintreeInstance) {
            braintreeInstance
                .teardown()
                .then(() => {
                    initializeBraintree();
                });
        } else {
            initializeBraintree();
        }

        GetLastBill().then(result => {
            if (!result.id) return;
            billingAddressId.current.value = result.id
            firstName.current.value = result.firstName;
            lastName.current.value = result.lastName;
            streetAddress.current.value = result.streetAddress;
            region.current.value = result.region;
            postalCode.current.value = result.postalCode;
            setCountry({value: result.countryCodeAlpha2, label: result.countryCodeAlpha2});
        });
    }, [])

    function collectData() {
        return {
            id: billingAddressId.current.value,
            firstName: firstName.current.value,
            lastName: lastName.current.value,
            streetAddress: streetAddress.current.value,
            region: region.current.value,
            postalCode: postalCode.current.value,
            countryCodeAlpha2: country.value,
            countryName: country.label,
        };
    }
    return (
        <div className="bg-gray p-3 m-2">
            <div>
                <div className="col bg-white p-3 min-200">
                    <h3>Платіжна адреса</h3>
                    <input ref={billingAddressId} type="number" hidden defaultValue={0} />
                    <div className="form-group rounded-0 required">
                        <label className="control-label" htmlFor="fname"><FontAwesomeIcon icon={faUser} />Ім'я</label>
                        <input ref={firstName} className="form-control rounded-0" type="text" id="fname" name="firstname" required />
                    </div>
                    <div className="form-group rounded-0 required">
                        <label className="control-label" htmlFor="lname"><FontAwesomeIcon icon={faUser} />Призвіще</label>
                        <input ref={lastName} className="form-control rounded-0" type="text" id="lname" name="lastname" required />
                    </div>
                    <div className="form-group rounded-0 required">
                        <label className="control-label" htmlFor="adr"><FontAwesomeIcon icon={faHome} />Адреса</label>
                        <input ref={streetAddress} className="form-control rounded-0" type="text" id="adr" name="address" required />
                    </div>
                    <div id="z-index-top" className="form-group rounded-0 required">
                        <label className="control-label" htmlFor="adr"><FontAwesomeIcon icon={faGlobe} />Країна</label>
                        <Select options={countries} value={country} onChange={(value) => changeHandler(value)} />
                    </div>
                    <div className="d-flex justify-content-between">
                        <div className="form-group rounded-0 required">
                            <label className="control-label" htmlFor="region">Область</label>
                            <input ref={region} className="form-control rounded-0" type="text" id="region" name="region" required />
                        </div>
                        <div className="form-group rounded-0 required">
                            <label className="control-label" htmlFor="postalCode">Індекс</label>
                            <input ref={postalCode} className="form-control rounded-0" type="text" id="postalCode" name="postalCode" required />
                        </div>
                    </div>
                </div>
            </div>
            <div>
                {
                    !process.env.NODE_ENV || process.env.NODE_ENV === 'development' ?
                        <div>

                            <h3>Тестова картка</h3>
                            <h5>5555555555554444</h5>
                        </div>
                        :
                        ''
                }
                <div id="braintree-drop-in-div" />

                <Button
                    className="braintreePayButton rounded-0 w-100"
                    type="primary"
                    disabled={!braintreeInstance}
                    onClick={() => {
                        if (braintreeInstance) {
                            braintreeInstance.requestPaymentMethod(
                                (error, payload) => {
                                    if (error) {
                                        console.error(error);
                                        onError()
                                    } else {
                                        setSended(true)
                                        onSuccess(collectData(), payload);
                                    }
                                });
                        }
                    }}
                >

                    Продовжити
                </Button>
            </div>
        </div>
    )
}
export default Braintree;