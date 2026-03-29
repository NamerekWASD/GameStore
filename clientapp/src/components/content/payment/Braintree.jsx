import { useEffect, useCallback, useMemo, useRef, useState } from "react";
import { Button } from "reactstrap";
import dropin from "braintree-web-drop-in"
import { GetLastBill } from "../../../utils/ApiRequests";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faGlobe, faHome, faUser } from "@fortawesome/free-solid-svg-icons";
import countryList from 'react-select-country-list'
import Select from 'react-select'
import { toast } from "react-toastify";
import { useTranslation } from 'react-i18next';

const Braintree = ({ sCartData, onSuccess, setSended, onError }) => {
    const { t } = useTranslation();
    const [braintreeInstance, setBraintreeInstance] = useState();

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

    const initializeBraintree = useCallback(() =>
        fetch('api/braintreePayment/getClientToken', {
                method: 'POST',
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json"
                },
            })
            .then(response => response.json())
            .then(clientToken => {
                dropin.create({
                    authorization: clientToken,
                    container: '#braintree-drop-in-div',
                    preselectVaultedPaymentMethod: true,
                }, function (error, instance) {
                    if (error) {

                        console.error(error)
                        braintreeInstance.teardown()
                            .then(() => {
                                initializeBraintree();
                            });
                    }
                    else {
                        setBraintreeInstance(instance);
                        GetLastBill().then(result => {
                            if (!result.id) return;
                            billingAddressId.current.value = result.id
                            firstName.current.value = result.firstName;
                            lastName.current.value = result.lastName;
                            streetAddress.current.value = result.streetAddress;
                            region.current.value = result.region;
                            postalCode.current.value = result.postalCode;
                            setCountry({ value: result.countryCodeAlpha2, label: result.countryCodeAlpha2 });
                        });

                    }
                })
            }), []);

    useEffect(() => {
        if (braintreeInstance)
            return;
        initializeBraintree();
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
    };

    const onPayButtonClick = () => {
        if (braintreeInstance) {
            braintreeInstance.requestPaymentMethod(
                (error, payload) => {
                    if (error) {
                        console.error(error);
                        onError();
                    } else {
                        setSended(true);
                        onRequestPaymentMethod(collectData(), payload);
                    }
                });
        }
    };
    const onRequestPaymentMethod = async (billData, payload) => {
        const requestInfo = `api/braintreePayment/create`;
        const body = {
            userEmail: sCartData.userEmail,
            games: sCartData.games,
            BillingAddress: billData,
            nonce: payload.nonce,
            paymentType: payload.type,
        }
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
            pending: t('payment.pending'),
        })
        const response = await promise;
        const result = await response.json()
        if (!response.ok) {
            toast.dismiss(toastPromise);
            toast.error(result);
            setSended(false);
            return;
        }
        toast.success(t('payment.success'));
        onSuccess(result);
    };
    return (
        <div className="bg-gray p-3 m-2 text-black">
            <div>
                <div className="col bg-white p-3 min-200">
                    <h3>{t('payment.billingAddress')}</h3>
                    <input ref={billingAddressId} type="number" hidden defaultValue={0} />
                    <div className="form-group rounded-0 required">
                        <label className="control-label" htmlFor="fname"><FontAwesomeIcon icon={faUser} />{t('payment.firstName')}</label>
                        <input ref={firstName} className="form-control rounded-0" type="text" id="fname" name="firstname" required />
                    </div>
                    <div className="form-group rounded-0 required">
                        <label className="control-label" htmlFor="lname"><FontAwesomeIcon icon={faUser} />{t('payment.lastName')}</label>
                        <input ref={lastName} className="form-control rounded-0" type="text" id="lname" name="lastname" required />
                    </div>
                    <div className="form-group rounded-0 required">
                        <label className="control-label" htmlFor="adr"><FontAwesomeIcon icon={faHome} />{t('payment.address')}</label>
                        <input ref={streetAddress} className="form-control rounded-0" type="text" id="adr" name="address" required />
                    </div>
                    <div id="z-index-top" className="form-group rounded-0 required">
                        <label className="control-label" htmlFor="adr"><FontAwesomeIcon icon={faGlobe} />{t('payment.country')}</label>
                        <Select options={countries} value={country} onChange={(value) => changeHandler(value)} />
                    </div>
                    <div className="d-flex justify-content-between">
                        <div className="form-group rounded-0 required">
                            <label className="control-label" htmlFor="region">{t('payment.region')}</label>
                            <input ref={region} className="form-control rounded-0" type="text" id="region" name="region" required />
                        </div>
                        <div className="form-group rounded-0 required">
                            <label className="control-label" htmlFor="postalCode">{t('payment.postalCode')}</label>
                            <input ref={postalCode} className="form-control rounded-0" type="text" id="postalCode" name="postalCode" required />
                        </div>
                    </div>
                </div>
            </div>
            <div>
                {
                    !process.env.NODE_ENV || process.env.NODE_ENV === 'development' ?
                        <div>

                            <h3>{t('payment.testCard')}</h3>
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
                    onClick={onPayButtonClick}
                >

                    {t('payment.continue')}
                </Button>
            </div>
        </div>
    )
}
export default Braintree;