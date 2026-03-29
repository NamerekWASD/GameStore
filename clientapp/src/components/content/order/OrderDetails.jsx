import { useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useNavigate, useSearchParams } from "react-router-dom";
import { GetOrder } from "../../../utils/ApiRequests";
import { navigateToDetails } from "../../../utils/Navigation";
import Loading from "../../../utils/Loading";
import { formatDate, formatCurrency } from '../../../utils/i18nHelpers';
const OrderDetails = () => {
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();
    const [order, setOrder] = useState();

    const { t } = useTranslation();

    const createOrder = async () => {
        GetOrder(searchParams.get('orderNumber'), true, navigate).then(result => {
            result.createdRaw = result.created;
            setOrder(result);
        });
    };

    useEffect(() => {
        createOrder()
    }, []);

    return (
        <main>
        {
            order ?
            <div className="container mt-5 p-0">
                <div className="bg-success p-2 d-flex justify-content-between align-items-center">
                    <div>
                        <h3 className="text-center text-white">{t('order.title')} <span className="fw-bolder">{t('order.orderNumber', { number: order.orderNumber })}</span></h3>
                        <h5 className="text-center">{formatDate(order.createdRaw)}</h5>
                    </div>
                </div>
                <table className="responsive-table">
                    <thead>
                        <tr>
                            <th aria-label="Game image">{t('order.image')}</th>
                            <th>{t('order.game')}</th>
                            <th>{t('order.platform')}</th>
                            <th style={{ flexBasis: '15%' }}>{t('order.copyType')}</th>
                            <th>{t('order.data')}</th>
                            <th>{t('order.price')}</th>
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
                                        <td >{formatCurrency(item.price)}</td>
                                    </tr>
                                )
                            })
                        }

                    </tbody>
                </table>

                <hr />
                <h2 className="text-end"><span>{t('order.totalLabel')} </span><span>{formatCurrency(order.total)}</span></h2>
            </div>
            :
            <Loading />
        }
        </main>
    )
}
export default OrderDetails;