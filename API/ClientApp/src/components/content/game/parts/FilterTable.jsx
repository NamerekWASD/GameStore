import { useEffect, useMemo, useState } from "react";
import { useSearchParams } from "react-router-dom";
import { loadFilterData } from "../../../../utils/ApiRequests";

const FilterTable = ({setGames}) => {
    const [searchParams] = useSearchParams();
    const [searchFilters, setSearchFilters] = useState({
        searchQuery: '',
        copyTypeNames: [],
        platformIds: [],
        genreIds: [],
        regionIds: [],
        developerId: 0,
        publisherId: 0,
        dateFrom: new Date(),
        dateTo: new Date(1970, 0, 1),
        priceFrom: 0,
        priceTo: 1000,
        isAvailable: true,
        isDiscounted: false,
        isHotOffer: false
    });
    const [filterData, setFilterData] = useState({
        copyTypeNames: [''],
        platforms: [{id: 0, name: ''}],
        genres: [{id: 0, name: ''}],
        developers: [{id: 0, name: ''}],
        publishers: [{id: 0, name: ''}],
    })

    useEffect(() => {
        loadFilterData().then(result => setFilterData(result));
    }, [])

    const reset = () => {

    }

    const applySettings = () => {

    }

    const processInput = (property, id, value) => {

    }

    const renderGenres = () => {
        return (
            <>
                {
                    searchParams.get('id') ? '' :
                        <div className="mb-1 bg-gray p-3">
                            <h5 className="text-center">Жанри</h5>
                            <div className="d-grid grid-2">
                                {
                                    filterData.genres.map((genre) => {
                                        return (
                                            <div key={genre.id}>
                                                <label>
                                                    <input id={"genre|" + genre.id} type="checkbox"
                                                        defaultValue={false}
                                                        onChange={(e) =>  processInput('genres', genre.id, e.target.value)} />
                                                    <label htmlFor={"genre|" + genre.id}>{genre.name}</label>
                                                </label>
                                            </div>
                                        )
                                    })
                                }
                            </div>
                        </div>
                }
            </>
        )
    }
    const memoGenres = useMemo(() => renderGenres(), [filterData])

    return (
        <div className='sticky-top ms-3' style={{ zIndex: "0", top: '55px', width: '350px' }}>
            <div className="mb-1 bg-gray p-3">
                <h5 className="text-center">Ціна</h5>
                <div className="p-3 d-flex justify-content-around gap-2">
                    <div className="bg-white">
                        <span className="px-1">Від</span>
                        <input
                            className="border-0 price no-outline" type="number"
                            min={0} max={1000}
                            id="minPrice" step="5" defaultValue="0"
                        />
                    </div>
                    <div className="bg-white">
                        <span className="px-1">До</span>
                        <input
                            className="border-0 price no-outline" type="number"
                            min={0} max={1000}
                            id="maxPrice" step="5" defaultValue="100"
                        />
                    </div>
                </div>
            </div>
            {memoGenres}
            <div className="mb-1 bg-gray p-3">
                <h5 className="text-center">Дата виходу</h5>
                <div className="p-3 d-flex justify-content-around gap-2">
                    <div className="bg-white">
                        <span className="px-1">Від</span>
                        <input
                            className="border-0 no-outline" type="number" min="1990"
                            max={new Date().getFullYear()} step="1" defaultValue="1990" />
                    </div>
                    <div className="bg-white">
                        <span className="px-1">До</span>
                        <input
                            className="border-0 no-outline" type="number" min="1990"
                            max={new Date().getFullYear()} step="1" defaultValue={new Date().getFullYear()} />
                    </div>
                </div>
            </div>
            <button className="btn rounded-0 btn-outline-danger w-100 mb-1" onClick={() => reset()}>Скинути</button>
            <button className="btn rounded-0 btn-outline-success w-100" onClick={() => applySettings()}>Підтвердити</button>
        </div>
    )
}

export default FilterTable;