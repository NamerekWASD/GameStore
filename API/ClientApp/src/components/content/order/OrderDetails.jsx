import React, { useEffect, useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { loadOrder } from "../../../utils/ApiRequests";
import { navigateToDetails } from "../../../utils/Navigation";
const OrderDetails = () => {
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();
    const [orders, setOrders] = useState();

    useEffect(() => {
        if (!orders) {
            loadOrder(searchParams.get('orderId'), true, navigate).then(result => {
                setOrders(result);
            });
        }
    }, [orders])

    return (
        <main>
            <div className="container mt-5 p-0">
                <div className="bg-success p-2">
                    <h3 className="text-center text-white">Замовлення <span className="fw-bolder">№{orders ? orders.id : ''}</span></h3>
                </div>
                <table className="responsive-table">
                    <thead>
                        <tr>
                            <th></th>
                            <th>Гра</th>
                            <th>Платформа</th>
                            <th style={{flexBasis: '15%'}}>Тип копії</th>
                            <th>Дані</th>
                            <th>Ціна</th>
                        </tr>
                    </thead>
                    <tbody>
                        {
                            orders ?
                                orders.copies.map((item, index) => {
                                    return (
                                        <tr key={item.id} className="pointer" onClick={() => navigateToDetails(item.copy.game, navigate)}>
                                            <td>
                                                <img src={item.copy.game.image.path} alt={item.copy.game.title} style={{ maxWidth: '100px' }} />
                                            </td>
                                            <td className="fw-bold">#{index + 1} {item.copy.game.title}</td>
                                            <td>{item.copy.game.platform}</td>
                                            <td>{item.copy.game.copyType}</td>
                                            <td style={{ whiteSpace: 'pre-line'}}>{item.copy.data}</td>
                                            <td >{item.price}<sup>$</sup></td>
                                        </tr>
                                    )
                                })
                                :
                                <>

                                </>
                        }

                    </tbody>
                </table>
            </div>
        </main>
    )
}
export default OrderDetails;