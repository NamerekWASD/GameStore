import { useTranslation } from "react-i18next";

const ModalDelete = ({ refModal, onAcceptDelete, game }) => {
    const { t } = useTranslation();
    return (
        <div ref={refModal} className="modal">
            <div className="modal-content rounded-0">
                <div className="modal-header bg-danger rounded-0 d-block p-3">
                    <h3 className="text-center">{t('manager.deleteTitle')}</h3>
                    <h3 className="text-black text-center">{game ? game.title : ''}</h3>
                </div>
                <div className="p-3">

                    <button type="button" className="w-100 btn btn-outline-success rounded-0" onClick={onAcceptDelete}>{t('manager.confirm')}</button>
                    <hr />
                    <button type="button" className="w-100 btn btn-outline-dark rounded-0" onClick={() => refModal.current.style.display = 'none'}>{t('manager.cancelDelete')}</button>
                </div>
            </div>
        </div>
    )
}
export default ModalDelete;