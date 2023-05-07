const ModalDelete = ({ refModal, onAcceptDelete, game }) => {
    return (
        <div ref={refModal} className="modal">
            <div className="modal-content rounded-0">
                <div className="modal-header bg-danger rounded-0 d-block p-3">
                    <h3 className="text-center">Видалити продукт</h3>
                    <h3 className="text-black text-center">{game ? game.title : ''}</h3>
                </div>
                <div className="p-3">

                    <button type="button" className="w-100 btn btn-outline-success rounded-0" onClick={onAcceptDelete}>Підтвердити</button>
                    <hr />
                    <button type="button" className="w-100 btn btn-outline-dark rounded-0" onClick={() => refModal.current.style.display = 'none'}>Я передумав...</button>
                </div>
            </div>
        </div>
    )
}
export default ModalDelete;