import { useEffect, useRef, useState } from "react";
import { useNavigate } from "react-router-dom"
import { navigateToDetails } from "../../../../utils/Navigation";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCircleDot } from "@fortawesome/free-solid-svg-icons";
import Price from "./Price";
import { POSTER } from "../../../../utils/Constants";
import { isMobile } from 'react-device-detect';
import { VideoContainer } from "./VideoContainer";

const emptyArray = [1, 2, 3, 4, 5];

const Carousel = ({ games }) => {
    const carousel = useRef(null);
    const container = useRef(null);
    const timeoutRef = useRef(null);
    const [isPanorama, setIsPanorama] = useState(window.innerWidth > 800);
    const [containerSize, setContainerSize] = useState({ width: 0, height: 0 })
    const navigate = useNavigate();
    const [sliderIndex, setSliderIndex] = useState(0);
    const maxItems = { current: 15 };
    const itemIndex = { current: -1 };

    useEffect(() => {
        window.addEventListener('resize', checkIsPanorama)
        function checkIsPanorama() {
            setIsPanorama(window.innerWidth > 800)

            setContainerSize(prevState => ({
                ...prevState,
                width: carousel.current.offsetWidth,
                height: carousel.current.offsetHeight,
            }));
        }
        return () => {
            window.removeEventListener('resize', checkIsPanorama);
        }
    }, [])

    const clearTimeOut = () => {
        if (timeoutRef.current) {
            clearTimeout(timeoutRef.current);
        }
    }

    const startCarousel = () => {
        clearTimeOut();
        timeoutRef.current = setTimeout(() => {
            setSliderIndex(prevIndex => prevIndex + 1 >= maxItems.current / 3 ? 0 : prevIndex + 1);
        }, 5000);
    }

    useEffect(() => {
        async function updateMaxItems() {
            await calculateMaxItems();
            startCarousel();
        }
        updateMaxItems();
    }, [games.length, calculateMaxItems, startCarousel])

    async function calculateMaxItems() {
        itemIndex.current = -1;
        const reducer = (games.length % 3);
        maxItems.current = Math.min(games.length - reducer, maxItems.current);
    }

    useEffect(() => {
        startCarousel()
        return () => clearTimeOut();
    }, [sliderIndex])

    useEffect(() => {
        setSliderIndex(0);
    }, [isPanorama])

    useEffect(() => {
        window.addEventListener('blur', clearTimeOut);
        window.addEventListener('focus', startCarousel);
        return () => {
            window.removeEventListener('blur', clearTimeOut);
            window.removeEventListener('focus', startCarousel);
        }
    }, [])

    function renderGame() {
        itemIndex.current++;
        const game = games[itemIndex.current];
        return (
            <>
                {
                    game ? <div className="pointer width-inherit height-inherit"
                        onClick={() => navigateToDetails(game, navigate)} >
                        <h5 className="position-absolute text-white p-2 m-2 responsive-carousel-title">{game.title}</h5>
                        {
                            game.discountPrice ?
                                <div className="position-absolute text-white m-2 carousel-item-price"><Price item={game} priceClassName="p-1" discountClassName="p-1" /></div>
                                : <></>
                        }
                        <img className="width-inherit height-inherit responsive-image"
                            src={game.images.find(value => value.type.name === POSTER)?.path}
                            alt={game.title}
                            style={isMobile || !isPanorama ? { width: containerSize.width + 'px', height: containerSize.height + 'px' } : {}} />
                    </div>
                        :
                        ""
                }
            </>

        )
    }
    return (
        <>
            <div className="video-container">
                {
                    isMobile || !isPanorama ?
                        ''
                        :
                        <VideoContainer

                        />
                }
                <div ref={container} className="d-flex flex-column justify-content-center align-items-center content bg-dark px-3" style={{ maxWidth: '100vw', height: '80vh' }}>
                    <div ref={carousel} className="overflow-hidden content" style={isMobile || !isPanorama ? { width: '80%', height: '70%' } : { maxWidth: '750px' }}>
                        <div className="carousel-track d-flex flex-row" style={{ transform: `translateX(-${sliderIndex * 100}%)` }}>
                            {
                                emptyArray.slice(0, maxItems.current / 3).map((_, index) => {
                                    return (
                                        <div key={index} className="d-flex flex-row my-carousel-item carousel-slide" style={isMobile || !isPanorama ? {} : { height: '600px' }}>
                                            {
                                                isMobile || !isPanorama ?
                                                    <div className="overflow-hidden position-relative my-carousel-content" style={{ height: '500px' }}>
                                                        {renderGame()}
                                                    </div>
                                                    :
                                                    <>
                                                        <div className="d-flex flex-column">
                                                            <div className="overflow-hidden position-relative my-carousel-content" style={{ width: '250px' }}>
                                                                {renderGame()}
                                                            </div>
                                                            <div className="overflow-hidden position-relative my-carousel-content" style={{ width: '250px' }}>
                                                                {renderGame()}
                                                            </div>
                                                        </div>
                                                        <div className="overflow-hidden position-relative my-carousel-content" style={{ width: '500px' }}>
                                                            {renderGame()}
                                                        </div>
                                                    </>
                                            }
                                        </div>
                                    )
                                })
                            }
                        </div>
                    </div>

                    <div className="d-flex justify-content-center">
                        {
                            emptyArray.slice(0, maxItems.current / 3).map((_, index) => {
                                return (
                                    <div key={index} className="pointer mx-2">
                                        <FontAwesomeIcon
                                            className={index === sliderIndex ? 'my-carousel-bullet active' : 'my-carousel-bullet'}
                                            icon={faCircleDot}
                                            onClick={() => setSliderIndex(index)} />
                                    </div>
                                )
                            })
                        }
                    </div>
                </div>
            </div>
        </>
    )
}

export default Carousel;