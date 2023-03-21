import { useRef } from "react";
import { toast } from "react-toastify";
import { subscribeOnGame } from "../../../../utils/ApiRequests";

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
                    <h3 className="text-center">Підписатись на гру 
                        <span id="close-modal" className="close-modal position-absolute end-0 me-3" onClick={() => refModal.current.style.display = 'none'}>&times;</span>
                    </h3>
                    <h3 className="text-center">{game ? game.title : 'Error'}</h3>
                </div>
                <form className='text-center' onSubmit={submit}>
                    <h1 className="fw-bold mb-1">Пошта</h1>
                    <div>
                        <input ref={email} type="email" className='text-input fs-5 mb-3 no-outline w-100 form-control rounded-0'
                            onChange={(e) => e.target.value} placeholder="example@domain.com"/>
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