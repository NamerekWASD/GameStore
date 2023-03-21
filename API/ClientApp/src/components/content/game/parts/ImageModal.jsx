import LoadingCircle from "../../../../utils/LoadingCircle";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faArrowLeftLong, faArrowRightLong, faCircleArrowLeft, faCircleArrowRight } from "@fortawesome/free-solid-svg-icons";
import $ from 'jquery'

const ImageModal = ({ images, showContent, setShowContent, setImageToShow, current }) => {

    const iterateItem = (index) => {
        const currentIndex = images.findIndex(item => item.id === current.id);
        var arrayIndex = currentIndex + index
        const nextItem = images[arrayIndex];
        if (nextItem) {
            setImageToShow(nextItem);
            return
        }
        arrayIndex = index === -1 ? images.length - 1 : 0
        setImageToShow(images[arrayIndex])
    }


    return (
        <>
            {
                showContent ?
                    <div className="modal-visible p-0 prevent-select">
                        <div className="position-absolute w-100 float-end" style={{ zIndex: 2 }}>
                            <span className="position-absolute close-modal text-center bg-danger"
                                style={{ width: '50px', height: '50px', transition: '.3s', right: '0' }}
                                onClick={() => setShowContent(false)}>&times;</span>
                        </div>

                        <div className="position-absolute rounded-0" style={{ top: '50px', transform: 'translateX(-50%)', left: '50%' }}>
                            <img src={current.path} alt={current.name} style={{ maxHeight: '70vh', maxWidth: '100vw', pointerEvents: 'none' }} />
                        </div>
                        <span onClick={() => iterateItem(-1)} className="h-100 position-absolute pointer"
                            style={{ left: '50px' }}>
                            <div className="position-relative" style={{ transform: 'translateY(-50%)', top: '50%' }}>
                                <FontAwesomeIcon icon={faCircleArrowLeft} size={"3x"} color={"white"} />
                            </div>
                        </span>
                        <span onClick={() => iterateItem(1)} className="h-100 position-absolute pointer"
                            style={{ right: '50px' }}>
                            <div className="position-relative" style={{ transform: 'translateY(-50%)', top: '50%' }}>
                                <FontAwesomeIcon icon={faCircleArrowRight} size={"3x"} color={"white"} />
                            </div>
                        </span>
                        <div className="position-absolute w-100 text-center d-flex align-items-center px-4" style={{ bottom: '50px' }}>
                            <FontAwesomeIcon icon={faArrowLeftLong} size={"4x"} color={"white"} onClick={() => {
                                const element = $("#image-container")
                                element.scrollLeft(element.scrollLeft() - 150);
                            }} />
                            <div id="image-container" className="d-flex flex-row overflow-scroll hide-scrollbar"
                                style={{ width: '100%' }}>
                                {
                                    images ? images.map(image => {
                                        return (
                                            <div key={image.id} className="pointer mx-3" style={{ height: '150px' }}
                                                onClick={() => setImageToShow(image)}>
                                                <img src={image.path} alt={image.name} className={"height-inherit " + (current === image ? "border border-2 border-danger" : "")} />
                                            </div>
                                        )
                                    })
                                        :
                                        <LoadingCircle />
                                }

                            </div>
                            <FontAwesomeIcon icon={faArrowRightLong} size={"4x"} color={"white"} onClick={() => {
                                const element = $("#image-container")
                                element.scrollLeft(element.scrollLeft() + 150);
                            }} />
                        </div>
                        <div className="w-100 h-100" onClick={() => setShowContent(false)}>

                        </div>
                    </div>
                    : ''
            }
        </>
    )
}
export default ImageModal;