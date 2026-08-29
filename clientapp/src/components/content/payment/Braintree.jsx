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
import "./Braintree.css";

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
                    // Hosted Fields render in a Braintree-origin iframe, so our stylesheet
                    // can't reach them - color values must be literal, matching
                    // --text-primary/--text-muted from index.css. Every key here mirrors
                    // one of drop-in's own default style keys (see its card-view.js) -
                    // the default ':focus' color is a separate rule from 'input' and
                    // wins while typing, so it must be overridden too or typed text
                    // stays black-on-dark while a field is focused.
                    card: {
                        overrides: {
                            styles: {
                                input: {
                                    color: '#f1f5f9',
                                    'font-size': '15px',
                                },
                                ':focus': {
                                    color: '#f1f5f9',
                                },
                                '::-webkit-input-placeholder': {
                                    color: '#94a3b8',
                                },
                                ':-moz-placeholder': {
                                    color: '#94a3b8',
                                },
                                '::-moz-placeholder': {
                                    color: '#94a3b8',
                                },
                                ':-ms-input-placeholder': {
                                    color: '#94a3b8',
                                },
                            },
                        },
                    },
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
        <div className="payment-layout">
            <div className="payment-card">
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
                    <Select classNamePrefix="rs" options={countries} value={country} onChange={(value) => changeHandler(value)} />
                </div>
                <div className="d-flex justify-content-between payment-region-postal">
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
            <div className="payment-card">
                <h3>{t('payment.payWithCard')}</h3>
                {
                    !process.env.NODE_ENV || process.env.NODE_ENV === 'development' ?
                        <div className="payment-test-card">
                            <h5>{t('payment.testCard')}</h5>
                            <p>5555555555554444</p>
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