import { useEffect, useRef, useState } from "react";
import { useSearchParams } from "react-router-dom";
import { loadFilterData, loadGamesByFilters } from "../../../../utils/ApiRequests";
import FieldCheckBoxSet from "./filter/FieldCheckBoxSet";
import $ from 'jquery'

const searchFiltersBlank = {
    searchQuery: '',
    regionIds: [],
    platformIds: [],
    genreIds: [],
    developerId: 0,
    publisherId: 0,
    dateFrom: new Date(1990, 0, 1),
    dateTo: new Date(),
    priceFrom: 0,
    priceTo: 1000,
    isAvailable: true,
    isDiscounted: false,
    isHotOffer: false
}

const FilterTable = ({ setGames, searchFilter }) => {
    const [searchFilters, setSearchFilters] = useState(searchFiltersBlank);

    const minPrice = useRef(null);
    const maxPrice = useRef(null);
    const minDate = useRef(null);
    const maxDate = useRef(null);

    const [filterData, setFilterData] = useState({
        copyTypes: [{ id: 0, name: '', platform: { id: 0, name: 0 }, availableRegions: [{ id: 0, name: 0 }] }],
        platforms: [{ id: 0, name: '' }],
        genres: [{ id: 0, name: '' }],
        developers: [{ id: 0, name: '' }],
        publishers: [{ id: 0, name: '' }],
        availableRegions: [{ id: 0, name: '' }]
    })

    useEffect(() => {
        if (searchFilter) {
            setSearchFilters(prevState => ({
                ...prevState,
                genreIds: searchFilter.genreIds
            }))
        }
        loadFilterData().then(result => setFilterData(result));
    }, [])

    const reset = () => {
        const element = $("#form-filter input[type='checkbox']");
        element.each(function () {
            this.checked = false;
        });

        minPrice.current.value = searchFiltersBlank.priceFrom
        maxPrice.current.value = searchFiltersBlank.priceTo
        minDate.current.value = searchFiltersBlank.dateFrom.getFullYear()
        maxDate.current.value = searchFiltersBlank.dateTo.getFullYear()

        setSearchFilters(searchFiltersBlank);
    }

    const applySettings = async () => {
        const data = await loadGamesByFilters(searchFilters);
        setGames(data);
    }

    const processInput = (id, value, parameter) => {
        if (value) {
            setSearchFilters(prevState => ({
                ...prevState,
                [parameter]: searchFilters[parameter].concat(id)
            }))
        }
        else {
            setSearchFilters(prevState => ({
                ...prevState,
                [parameter]: searchFilters[parameter].filter(genreId => genreId !== id)
            }))
        }
    }

    return (
        <div id="form-filter" className='ms-3' style={{ width: '350px' }}>

            <FieldCheckBoxSet
                fieldName={"Жанри"}
                propertyName={"genreIds"}
                array={filterData.genres}
                processInput={processInput}
                checked={searchFilter ? searchFilter.genreIds : undefined}
            />
            <FieldCheckBoxSet
                fieldName={"Доступні регіони"}
                propertyName={"regionIds"}
                array={filterData.availableRegions}
                processInput={processInput}
                checked={undefined}
            />
            <FieldCheckBoxSet
                fieldName={"Платформи"}
                propertyName={"platformIds"}
                array={filterData.platforms}
                processInput={processInput}
                checked={undefined}
            />
            <div className="mb-1 bg-gray p-3">
                <h5 className="text-center p-1">Ціна</h5>
                <div className="p-3 d-flex justify-content-around gap-2">
                    <div className="bg-white">
                        <span className="px-1">Від</span>
                        <input ref={minPrice}
                            className="border-0 price no-outline min-input" type="number"
                            min={0} max={1000} step="5"
                            defaultValue={searchFilters ? searchFilters.priceFrom : 0}
                            onChange={(e) => setSearchFilters(prevState => ({
                                ...prevState,
                                priceFrom: e.target.value
                            }))}
                        />
                    </div>
                    <div className="bg-white">
                        <span className="px-1">До</span>
                        <input ref={maxPrice}
                            className="border-0 price no-outline max-input" type="number"
                            min={0} max={1000} step="5"
                            defaultValue={searchFilters ? searchFilters.priceTo : 1000}
                            onChange={(e) => setSearchFilters(prevState => ({
                                ...prevState,
                                priceTo: e.target.value
                            }))}
                        />
                    </div>
                </div>
            </div>
            <div className="mb-1 bg-gray p-3">
                <h5 className="text-center p-1">Дата виходу</h5>
                <div className="p-3 d-flex justify-content-around gap-2">
                    <div className="bg-white">
                        <span className="px-1">Від</span>
                        <input ref={minDate}
                            className="border-0 no-outline min-input" type="number" min={1990} max={2100} step="1"
                            defaultValue={searchFilters ? searchFilters.dateFrom.getFullYear() : 1990}
                            onChange={(e) => setSearchFilters(prevState => ({
                                ...prevState,
                                dateFrom: new Date(e.target.value, 0, 1)
                            }))}
                        />
                    </div>
                    <div className="bg-white">
                        <span className="px-1">До</span>
                        <input ref={maxDate}
                            className="border-0 no-outline " type="number" min={1990} max={2100} step="1"
                            defaultValue={searchFilters ? searchFilters.dateTo.getFullYear() : 2100}
                            onChange={(e) => setSearchFilters(prevState => ({
                                ...prevState,
                                dateTo: new Date(e.target.value, 0, 1)
                            }))}
                        />
                    </div>
                </div>
            </div>
            <div className="mb-1 bg-gray p-3">
                <div className="form-group checkbox-unique">
                    <input id="isDiscounted" type="checkbox" name="isDiscounted"
                        onChange={(e) => {
                            setSearchFilters(prevState => ({
                                ...prevState,
                                isDiscounted: e.target.checked
                            }))
                        }}
                    />
                    <label htmlFor="isDiscounted">Зі знижкою</label>
                </div>
                <div className="form-group checkbox-unique">
                    <input id="isAvailable" type="checkbox" name="isAvailable"
                        onChange={(e) => {
                            setSearchFilters(prevState => ({
                                ...prevState,
                                isAvailable: e.target.checked
                            }))
                        }}
                    />
                    <label htmlFor="isAvailable">Доступні до продажу</label>
                </div>
                <div className="form-group checkbox-unique">
                    <input id="isHotOffer" type="checkbox" name="isHotOffer"
                        onChange={(e) => {
                            setSearchFilters(prevState => ({
                                ...prevState,
                                isHotOffer: e.target.checked
                            }))
                        }}
                    />
                    <label htmlFor="isHotOffer">Гарячі пропозиції</label>
                </div>
            </div>
            <button type="button" className="btn rounded-0 btn-outline-danger w-100 mb-1" onClick={() => reset()}>За замовчуванням</button>
            <button className="btn rounded-0 btn-outline-success w-100" onClick={() => applySettings()}>Підтвердити</button>
        </div>
    )
}

export default FilterTable;