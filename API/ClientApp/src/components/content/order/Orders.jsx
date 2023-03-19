import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { AppPaths } from "../../../utils/AppPaths";
import Order from "./Order";
const Orders = () => {
    const navigate = useNavigate();
    const [orders, setOrders] = useState();

    useEffect(() => {
        if (!orders) {
            loadOrders().then(result => setOrders(result));
        }
    }, [orders])

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
        console.log(responseBody);
        return responseBody;
    }

    const continueShopping = (e) => {
        navigate(AppPaths.gameSearch);
    }
    return (
        <main className="h-100">

            {
                orders && orders.length !== 0 ?
                    <table className="px-3 responsive-table">
                        <thead>
                            <tr>
                                <th>Номер Замовлення</th>
                                <th>Сума</th>
                                <th>Кількіть товарів</th>
                                <th>Дата замолення</th>
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
                            Ви поки що нічого не замовляли
                        </h3>
                        <button className="btn btn-outline-success rounded-0 btn-75px" onClick={continueShopping}>Продовжити покупки</button>
                    </div>
            }
        </main>
    )
}
export default Orders;