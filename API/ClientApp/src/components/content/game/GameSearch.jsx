import React, { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { loadGames, loadGamesByGenre, loadGenres } from "../../../utils/ApiRequests";
import { navigateToDetails } from "../../../utils/Navigation";
import './game.css'

const GameSearch = () => {
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();
    const [games, setGames] = useState([]);
    const [genres, setGenres] = useState([]);
    const [defaultList, setDefault] = useState();
    const genreRef = useRef(null);
    const minPrice = useRef(null);
    const maxPrice = useRef(null);

    const minDate = useRef(null);
    const maxDate = useRef(null);

    const order = {
        names: { name: 'title', type: 'desc' },
        newer: { name: 'released', type: 'asc' },
        older: { name: 'released', type: 'desc' },
        expensivest: { name: 'price', type: 'asc' },
        cheaper: { name: 'price', type: 'desc' },
        popularity: { name: 'sold', type: 'asc' }
    }
    const [sortType, setSortType] = useState(order.names);

    const memoGames = useMemo(() => renderGames(), [games, sortType]);

    useEffect(() => {
        genreRef.current.textContent = searchParams.get('genre') ?? 'Каталог';
        if (searchParams.get('id')) {
            loadGamesByGenre(searchParams.get('id')).then(result => {
                setGames(result);
                setDefault(result);
            });
        } else {
            loadGames(true).then(result => {
                setGames(result);
                setDefault(result);
            });
            loadGenres().then(result => setGenres(result));
        }

    }, [searchParams]);
    function applySettings() {
        var newList = defaultList.filter(game => {
            if (game.price < +minPrice.current.value || game.price > +maxPrice.current.value) {
                return false;
            }
            const year = game.released.split('-')[0];
            if (year < parseInt(minDate.current.value) || year > parseInt(maxDate.current.value)) {
                return false;
            }
            let someGenreChecked = genres.some(genre => genre.checked === true);
            if (someGenreChecked && !genres.some(filter => filter.checked && game.genres.some(gameGenre => filter.name === gameGenre))) {
                return false;
            }

            return true;
        });
        setGames(newList);
    }
    function renderGames() {
        const copy = games.sort(function (a, b) {
            var propertyA, propertyB;
            if (sortType.name === order.newer.name) {
                propertyA = new Date(Date.parse(a[sortType.name]));
                propertyB = new Date(Date.parse(b[sortType.name]));
            } else {
                propertyA = a[sortType.name];
                propertyB = b[sortType.name];
            }
            if (propertyA < propertyB) return -1;
            if (propertyA > propertyB) return 1;
            return 0
        });
        if (sortType.type === 'asc') {
            copy.reverse();
        }
        return (
            <>
                {
                    (copy.length !== 0) ?
                        copy.map((game, index) => {
                            return (
                                <div key={index} className="card rounded-0 p-4 w-100" style={{ cursor: 'pointer', height: '180px' }}
                                    onClick={() => navigateToDetails(game, navigate)} >
                                    <div className="no-gutters d-flex flex-row align-items-center gap-5">
                                        <div>
                                            <img src={game.imageURL} alt={game.title} style={{ width: '100px' }} />
                                        </div>
                                        <div className="flex-fill">
                                            <h6 className="card-title">{game.title}</h6>
                                            <h6 className="text-muted">{game.genres.join(' ')}</h6>
                                        </div>
                                        <div>
                                            <span className="fs-3">{game.price}<sup>$</sup></span>
                                        </div>
                                    </div>
                                </div>
                            )
                        }) : <h3>За вашим запитом не знайдено жодної гри</h3>
                }
            </>
        )
    }

    function renderGenres() {
        return (
            <>
                {
                    searchParams.get('id') ? '' :
                        <div className="mb-1 bg-gray p-3">
                            <h5 className="text-center">Жанри</h5>
                            <div className="d-grid grid-2">
                                {
                                    genres ? genres.map((genre) => {
                                        return (
                                            <div key={genre.id}>
                                                <label>
                                                    <input id={"genre|" + genre.id} type="checkbox"
                                                        defaultValue={false}
                                                        onChange={(e) => genre.checked = e.target.checked} />
                                                    <label htmlFor={"genre|" + genre.id}>{genre.name}</label>
                                                </label>
                                            </div>
                                        )
                                    })
                                        : ''
                                }
                            </div>
                        </div>
                }
            </>
        )
    }

    function reset() {

    }
    return (
        <div className="container h-100 mt-4">
            <div>
                <h1 ref={genreRef} className="fw-bolder"></h1>
                <div className="d-flex flex-row">
                    <div className="fw-bold">Знайдено {games.length} {games.length.toString().endsWith('1') ? 'гра' : 'гри'}</div>
                    <div className="ms-5">
                        Упорядкувати
                        <select className="ms-2 no-outline overflow-scroll" id="order-list"
                            style={{ width: '200px' }} onChange={(e) => {
                                setSortType(order[e.target.value])
                            }}>
                            <option value={"names"}>За назвою</option>
                            <option value={"newer"}>За датою, від нових</option>
                            <option value={"older"}>За датою, від старих</option>
                            <option value={"expensivest"}>За ціною, від дорогих</option>
                            <option value={"cheaper"}>За ціною, від дешевих</option>
                            <option value={"popularity"}>За популярністю</option>
                        </select>
                    </div>
                </div>
            </div>
            <div className="d-flex mt-5 flex-row gap-4">
                <div className="flex-fill h-100">
                    <div className="col">
                        {memoGames}
                    </div>
                </div>
                <div className='sticky-top ms-3' style={{ zIndex: "0", top: '55px', width: '350px' }}>
                    <div className="mb-1 bg-gray p-3">
                        <h5 className="text-center">Ціна</h5>
                        <div className="p-3 d-flex justify-content-around gap-2">
                            <div className="bg-white">
                                <span className="px-1">Від</span>
                                <input
                                    ref={minPrice}
                                    className="border-0 price no-outline" type="number"
                                    min={0} max={100}
                                    id="minPrice" step="5" defaultValue="0"
                                />
                            </div>
                            <div className="bg-white">
                                <span className="px-1">До</span>
                                <input
                                    ref={maxPrice}
                                    className="border-0 price no-outline" type="number"
                                    min={0} max={100}
                                    id="maxPrice" step="5" defaultValue="100"
                                />
                            </div>
                        </div>
                    </div>
                    {renderGenres()}
                    <div className="mb-1 bg-gray p-3">
                        <h5 className="text-center">Дата виходу</h5>
                        <div className="p-3 d-flex justify-content-around gap-2">
                            <div className="bg-white">
                                <span className="px-1">Від</span>
                                <input ref={minDate}
                                    className="border-0 no-outline" type="number" min="1990"
                                    max={new Date().getFullYear()} step="1" defaultValue="1990" />
                            </div>
                            <div className="bg-white">
                                <span className="px-1">До</span>
                                <input ref={maxDate}
                                    className="border-0 no-outline" type="number" min="1990"
                                    max={new Date().getFullYear()} step="1" defaultValue={new Date().getFullYear()} />
                            </div>
                        </div>
                    </div>
                    <button className="btn rounded-0 btn-outline-danger w-100 mb-1" onClick={() => reset()}>Скинути</button>
                    <button className="btn rounded-0 btn-outline-success w-100" onClick={() => applySettings()}>Підтвердити</button>
                </div>
            </div>
        </div>
    );
}
export default GameSearch;
