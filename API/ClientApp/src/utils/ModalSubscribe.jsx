import { useRef } from "react";
import { toast } from "react-toastify";
import { subscribeOnGame } from "./ApiRequests";

const ModalSubscribe = ({ refModal, game}) => {
    const email = useRef(null);

    const submit = async (e) => {
        e.preventDefault();
        const response = await subscribeOnGame(game.id).catch(err => toast.error(err));
        toast.promise(response, {
            pending: 'Зачекайте...',
            success: 'Готово, ви підписані!',
            error: 'Сталась помилка...'
        })
    };

    return (
        <div ref={refModal} className="modal">
            <div className="modal-content rounded-0">
                <div className="modal-header rounded-0 d-block p-3">
                    <h3 className="text-center">Підписатись на гру <strong>{game ? game.title : 'Error'}</strong>
                        <span id="close-modal" className="close-modal float-end" onClick={() => refModal.current.style.display = 'none'}>&times;</span>
                    </h3>
                </div>
                <form className='text-center' onSubmit={submit}>
                    <h1 className="fw-bold mb-1">Пошта</h1>
                    <div>
                        <input ref={email} type="email" className='text-input text-center fs-1 mb-3 no-outline'
                            onChange={(e) => e.target.value} />
                    </div>
                    <div className="modal-body p-2">
                        <button type='submit' className="btn btn-outline-success rounded-0 w-100">Підтвердити</button>
                    </div>
                </form>
            </div>
        </div>
    )
}
export default ModalSubscribe;