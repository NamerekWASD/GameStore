import { useRef } from "react";
import { toast } from "react-toastify";
import { SendEmailConfirmationCode } from "../../../../utils/ApiRequests";

const ModalEmailConfirmation = ({ refModal, refEmail, RememberMe, after }) => {
    const ConfirmationCode = useRef();

    const confirmEmail = async (e) => {
        e.preventDefault();
        const confirmationModel = {
            email: refEmail.current.value,
            code: ConfirmationCode.current.value,
            rememberMe: RememberMe ?? false
        };

        const response = await SendEmailConfirmationCode(confirmationModel);
        if (!response.ok) {
            toast.error(await response.text());
            return;
        }
        toast.success("Пошту підтверджено!")
        if (after) after(response)
    };

    return (
        <div ref={refModal} className="modal">
            <div className="modal-content rounded-0">
                <div className="modal-header rounded-0 d-block p-3">
                    <h3 className="text-center">Підтведіть електрону пошту
                        <span id="close-modal" className="close-modal float-end" onClick={() => refModal.current.style.display = 'none'}>&times;</span>
                    </h3>
                </div>
                <form className='text-center' onSubmit={confirmEmail}>
                    <h1 className="fw-bold mb-1">Код</h1>
                    <div className="form-group text-center">
                        <input ref={ConfirmationCode} type="text" className='text-input text-center fs-1 mb-3 form-control rounded-0 mx-auto border border-2 border-dark shadow-none'
                            minLength={5} maxLength={5} style={{ height: '70px', width: '150px' }}
                            onChange={(e) => {
                                if (e.target.value.length === 5) {
                                    confirmEmail(e)
                                }
                                return e.target.value;
                            }} />
                    </div>
                </form>
            </div>
        </div>
    )
}
export default ModalEmailConfirmation;