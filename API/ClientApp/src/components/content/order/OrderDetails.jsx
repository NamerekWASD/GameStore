import React, { useEffect, useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { GetOrder } from "../../../utils/ApiRequests";
import { navigateToDetails } from "../../../utils/Navigation";
import Loading from "../../../utils/Loading";
const OrderDetails = () => {
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();
    const [order, setOrder] = useState();

    useEffect(() => {
        const options = {
            day: 'numeric',
            month: 'long',
            year: 'numeric',
            hour: 'numeric',
            minute: 'numeric',
            hour12: false
        };

        const dateFormatter = new Intl.DateTimeFormat('uk-UA', options);

        GetOrder(searchParams.get('orderNumber'), true, navigate).then(result => {
            result.created = dateFormatter.format(new Date(result.created));
            setOrder(result);
        });
    }, [])
    return (
        <main>
        {
            order ?
            <div className="container mt-5 p-0">
                <div className="bg-success p-2">
                    <h3 className="text-center text-white">Замовлення <span className="fw-bolder">№{order.orderNumber}</span></h3>
                    <h5 className="text-center">{order.created}</h5>
                </div>
                <table className="responsive-table">
                    <thead>
                        <tr>
                            <th></th>
                            <th>Гра</th>
                            <th>Платформа</th>
                            <th style={{ flexBasis: '15%' }}>Тип копії</th>
                            <th>Дані</th>
                            <th>Ціна</th>
                        </tr>
                    </thead>
                    <tbody>
                        {
                                order.copies.map((item, index) => {
                                    return (
                                        <tr key={item.id} className="pointer" onClick={() => navigateToDetails(item.copy.game, navigate)}>
                                            <td>
                                                <img src={item.copy.game.image.path} alt={item.copy.game.title} style={{ maxWidth: '100px' }} />
                                            </td>
                                            <td className="fw-bold">#{index + 1} {item.copy.game.title}</td>
                                            <td>{item.copy.game.platform}</td>
                                            <td>{item.copy.game.copyType}</td>
                                            <td style={{ whiteSpace: 'pre-line' }}>{item.copy.data}</td>
                                            <td >{item.price}$</td>
                                        </tr>
                                    )
                                })
                        }

                    </tbody>
                </table>

                <hr />
                <h2 className="text-end"><span>Загалом: </span><span>{order.total}</span>$</h2>
            </div>
            :
            <Loading />
        }
        </main>
    )
}
export default OrderDetails;