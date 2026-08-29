import { useEffect, useMemo, useRef, useState } from "react";
import { useNavigate } from "react-router-dom"
import { navigateToDetails } from "../../../../utils/Navigation";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCircleDot } from "@fortawesome/free-solid-svg-icons";
import Price from "./Price";
import { POSTER } from "../../../../utils/Constants";
import { isMobile } from 'react-device-detect';
import { VideoContainer } from "./VideoContainer";

function chunk(array, size) {
    const result = [];
    for (let i = 0; i < array.length; i += size) {
        result.push(array.slice(i, i + size));
    }
    return result;
}

const Carousel = ({ games }) => {
    const carousel = useRef(null);
    const container = useRef(null);
    const timeoutRef = useRef(null);
    const [isPanorama, setIsPanorama] = useState(window.innerWidth > 800);
    const [containerSize, setContainerSize] = useState({ width: 0, height: 0 })
    const navigate = useNavigate();
    const [sliderIndex, setSliderIndex] = useState(0);
    const slides = useMemo(() => chunk(games, 3), [games]);

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
            setSliderIndex(prevIndex => prevIndex + 1 >= slides.length ? 0 : prevIndex + 1);
        }, 5000);
    }

    useEffect(() => {
        setSliderIndex(0);
    }, [slides.length])

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

    function renderGame(game) {
        if (!game) return null;
        const poster = game.images?.find(value => value.type?.name === POSTER);
        if (!poster) return null;
        return (
            <div className="pointer width-inherit height-inherit"
                onClick={() => navigateToDetails(game, navigate)} >
                <h5 className="position-absolute text-white p-2 m-2 responsive-carousel-title">{game.title}</h5>
                {
                    game.discountPrice ?
                        <div className="position-absolute text-white m-2 carousel-item-price"><Price item={game} priceClassName="p-1" discountClassName="p-1" /></div>
                        : <></>
                }
                <img className="width-inherit height-inherit responsive-image"
                    src={poster.path}
                    alt={game.title}
                    style={{ width: containerSize.width + 'px', height: containerSize.height + 'px' }} />
            </div>
        )
    }

    function renderCard(game, extraClassName = '') {
        if (!game) return null;
        const poster = game.images?.find(value => value.type?.name === POSTER);
        if (!poster) return null;
        return (
            <div className={`pointer carousel-card ${extraClassName}`.trim()}
                onClick={() => navigateToDetails(game, navigate)}>
                <img className="carousel-card-image" src={poster.path} alt={game.title} />
                <div className="carousel-card-gradient" />
                <div className="carousel-card-info">
                    <h5 className="carousel-card-title">{game.title}</h5>
                    {
                        game.discountPrice ?
                            <Price item={game} priceClassName="p-1" discountClassName="p-1" />
                            : null
                    }
                </div>
            </div>
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
                                slides.map((slide, index) => {
                                    return (
                                        <div key={index} className={isMobile || !isPanorama ? "d-flex flex-row my-carousel-item carousel-slide" : "d-flex flex-row my-carousel-item carousel-slide carousel-hero-row"}>
                                            {
                                                isMobile || !isPanorama ?
                                                    <div className="overflow-hidden position-relative my-carousel-content" style={{ height: '500px' }}>
                                                        {renderGame(slide[0])}
                                                    </div>
                                                    :
                                                    <>
                                                        {renderCard(slide[0], 'carousel-hero-main')}
                                                        <div className="carousel-hero-side">
                                                            {renderCard(slide[1])}
                                                            {renderCard(slide[2])}
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
                            slides.map((_, index) => {
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