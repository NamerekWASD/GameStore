import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom"
import { navigateToDetails } from "../../../../utils/Navigation";
import $ from 'jquery'
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCircle } from "@fortawesome/free-solid-svg-icons";
import Price from "./Price";

const totalWidth = 750;
const emptyArray = [1, 2, 3, 4, 5];
const timeout = { current: null };

const Carousel = ({ games }) => {
    const navigate = useNavigate();
    const [sliderIndex, setSliderIndex] = useState(0);
    const maxItems = { current: 15 };
    const myIndex = { current: -1 };

    calculateMaxItems()
    useEffect(() => {
        calculateMaxItems()
    }, [games])

    const clearTimeOut = () => {
        if (typeof timeout.current !== "undefined") {
            clearTimeout(timeout.current);
        }
    }

    function calculateMaxItems() {
        myIndex.current = -1;
        const reducer = (games.length % 3);
        maxItems.current = Math.min(games.length - reducer, maxItems.current)
    }

    const startCarousel = () => {
        if (typeof timeout.current !== "undefined") {
            clearTimeout(timeout.current);
        }
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
        startCarousel()
    }, [sliderIndex])

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
                    game ? <div className="pointer width-inherit"
                        onClick={() => navigateToDetails(game, navigate)} >
                        <h5 className="position-absolute text-white p-2 m-2 responsive-carousel-title">{game.title}</h5>
                        {
                            game.discountPrice ?
                                <div className="position-absolute text-white m-2 carousel-item-price"><Price item={game} priceClassName="p-1" discountClassName="p-1" /></div>
                                : <></>
                        }
                        <img className="width-inherit responsive-image" src={game.image.path} alt={game.title} />
                    </div>
                        :
                        ""
                }
            </>

        )
    }
    return (
        <>
            <div className="d-flex justify-content-center">
                <div className="d-flex flex-row overflow-hidden" style={{ width: totalWidth }}>
                    {
                        emptyArray.slice(0, maxItems.current / 3).map((_, index) => {
                            return (
                                <div key={index} className="d-flex flex-row my-carousel-item">
                                    <div className="d-flex flex-column">
                                        <div className="overflow-hidden position-relative my-carousel-content" style={{ width: '250px', height: '300px' }}>
                                            {renderGame()}
                                        </div>
                                        <div className="overflow-hidden position-relative my-carousel-content" style={{ width: '250px', height: '300px' }}>
                                            {renderGame()}
                                        </div>
                                    </div>
                                    <div className="overflow-hidden position-relative my-carousel-content" style={{ width: '500px', height: '600px' }}>
                                        {renderGame()}
                                    </div>
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
                            <div key={index} className="pointer ms-1"><FontAwesomeIcon className="my-carousel-bullet" icon={faCircle} color="white" onClick={() => setSliderIndex(index)} /></div>
                        )
                    })
                }
            </div>
        </>
    )
}

export default Carousel;