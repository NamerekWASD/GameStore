import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { AppPaths } from "../../../utils/AppPaths";
import LoadingCircle from "../../../utils/LoadingCircle";
import Order from "./Order";
import { useTranslation } from 'react-i18next';
const Orders = () => {
    const navigate = useNavigate();
    const { t } = useTranslation();
    const [orders, setOrders] = useState(undefined);

    useEffect(() => {
            loadOrders()
    }, [])

    async function loadOrders() {
        const requestInfo = `api/order`;
        const requestInit = {
            method: 'GET',
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
        };

        const response = await fetch(requestInfo, requestInit);
        const responseBody = await response.json();
        setOrders(responseBody);
    }

    const continueShopping = (e) => {
        navigate(AppPaths.gameCatalog);
    }
    return (
        <main className="h-100">

            {
                orders ? (
                    orders.length !== 0 ?
                        <table className="px-3 responsive-table">
                            <thead>
                                <tr>
                                    <th>{t('orders.table.orderNumber')}</th>
                                    <th>{t('orders.table.amount')}</th>
                                    <th>{t('orders.table.itemCount')}</th>
                                    <th>{t('orders.table.date')}</th>
                                </tr>
                            </thead>
                            <tbody>
                                {
                                    orders.map(order => {
                                        return (
                                            <Order key={order.id} data={order} />
                                        )
                                    })
                                }
                            </tbody>
                        </table>
                        :
                        <div className="p-3 text-center">
                            <h3>
                                {t('orders.emptyMessage')}
                            </h3>
                            <button className="btn btn-outline-success rounded-0 btn-75px" onClick={continueShopping}>{t('orders.continueShopping')}</button>
                        </div>
                )
                    :
                    <LoadingCircle />
            }
        </main>
    )
}
export default Orders;