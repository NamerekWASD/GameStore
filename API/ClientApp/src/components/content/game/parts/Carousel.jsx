import { useEffect, useRef, useState } from "react";
import { useNavigate } from "react-router-dom"
import { navigateToDetails } from "../../../../utils/Navigation";
import $ from 'jquery'
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCircleDot } from "@fortawesome/free-solid-svg-icons";
import Price from "./Price";
import { PORTRAIT } from "../../../../utils/Constants";
import { isMobile } from 'react-device-detect';
import { VideoContainer } from "./VideoContainer";

const emptyArray = [1, 2, 3, 4, 5];
const timeout = { current: null };

const Carousel = ({ games }) => {
    const carousel = useRef(null);
    const [isPanorama, setIsPanorama] = useState(window.innerWidth > 800);
    const [containerSize, setContainerSize] = useState({ width: 0, height: 0 })
    const navigate = useNavigate();
    const [sliderIndex, setSliderIndex] = useState(0);
    const maxItems = { current: 15 };
    const myIndex = { current: -1 };

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
        if (typeof timeout.current !== "undefined") {
            clearTimeout(timeout.current);
        }
    }

    const startCarousel = () => {
        if (typeof timeout.current !== "undefined") {
            clearTimeout(timeout.current);
        }
        const totalWidth = carousel.current.offsetWidth;
        if (sliderIndex >= maxItems.current / 3) {
            $('.my-carousel-item').animate({ left: `0px` }, 500);
            setSliderIndex(0);
            return;
        }
        $('.my-carousel-item').animate({ left: `-${totalWidth * sliderIndex}px` }, 500);
        $('.my-carousel-bullet').css({ 'color': 'gray' })
        $('.my-carousel-bullet').eq(sliderIndex).css({ 'color': 'white' })
        timeout.current = setTimeout(() => {
            setSliderIndex(sliderIndex + 1);
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
        myIndex.current = -1;
        const reducer = (games.length % 3);
        maxItems.current = Math.min(games.length - reducer, maxItems.current);
    }

    useEffect(() => {
        startCarousel()
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
        myIndex.current++;
        const game = games[myIndex.current];
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
                            src={game.images.find(value => value.type.name === PORTRAIT).path}
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
                <div className="d-flex flex-column justify-content-center align-items-center content bg-dark px-3" style={{ height: '80vh' }}>
                    <div ref={carousel} className="d-flex flex-row overflow-hidden content" style={isMobile || !isPanorama ? { width: '80%', height: '70%' } : { maxWidth: '750px' }}>
                        {
                            emptyArray.slice(0, maxItems.current / 3).map((_, index) => {
                                return (
                                    <div key={index} className="d-flex flex-row my-carousel-item" style={isMobile || !isPanorama ? {} : { height: '600px' }}>
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

                    <div className="d-flex justify-content-center">
                        {
                            emptyArray.slice(0, maxItems.current / 3).map((_, index) => {
                                return (
                                    <div key={index} className="pointer mx-2"><FontAwesomeIcon className="my-carousel-bullet" icon={faCircleDot} color="white" onClick={() => setSliderIndex(index)} /></div>
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