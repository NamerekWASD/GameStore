import { useEffect, useRef, useState } from "react";
import { GetFilterData } from "../../../../../utils/ApiRequests";
import FieldCheckBoxSet from "./FieldCheckBoxSet";
import $ from 'jquery'
import { scrollToTop } from "../../../../Layout";
import Select from 'react-select';

const FilterTable = ({ setPage, setSearchFilters, searchFilters, showGenre }) => {
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
        GetFilterData().then(result => {
            result.tags = result.tags.map(value => ({ label: value.name, value: value.id }))
            result.developers = result.developers.map(value => ({ label: value.name, value: value.id }))
            result.publishers = result.publishers.map(value => ({ label: value.name, value: value.id }))
            setFilterData(result)
        });
    }, [])

    const reset = () => {
        const element = $("#form-filter input[type='checkbox']");
        element.each(function () {
            this.checked = false;
        });

        minPrice.current.value = 0
        maxPrice.current.value = 100
        minDate.current.value = new Date().getFullYear()
        maxDate.current.value = 1990

        setSearchFilters({});
        scrollToTop();
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
        setPage(0);
    }

    return (
        <div id="form-filter" style={{ minWidth: '400px' }}>

            {
                showGenre ?
                <FieldCheckBoxSet
                fieldName={"Жанри"}
                propertyName={"genreIds"}
                array={filterData.genres}
                processInput={processInput}
                searchFilters={searchFilters}
            />
            : ''
            }
            <FieldCheckBoxSet
                fieldName={"Доступні регіони"}
                propertyName={"regionIds"}
                array={filterData.availableRegions}
                processInput={processInput}
                searchFilters={searchFilters}
            />
            <FieldCheckBoxSet
                fieldName={"Платформи"}
                propertyName={"platformIds"}
                array={filterData.platforms}
                processInput={processInput}
                searchFilters={searchFilters}
            />
            <div className="mb-1 bg-gray p-3">
                <h5 className="text-center p-1">Мітки</h5>
                <div>
                    <Select className="rounded-all-0 text-dark"
                        isMulti
                        options={filterData ? filterData.tags : undefined}
                        onChange={(value) => {
                            setSearchFilters(prevState => ({
                                ...prevState,
                                tagIds: value.map(select => select.value)
                            }))
                            setPage(0);
                        }}
                    />
                </div>
            </div>
            <div className="mb-1 bg-gray p-3">
                <h5 className="text-center p-1">Розробник</h5>
                <div>
                    <Select className="rounded-all-0 text-dark"
                        isClearable
                        options={filterData ? filterData.developers : undefined}
                        onChange={(value) => {
                            setSearchFilters(prevState => ({
                                ...prevState,
                                developerId: value ? value.value : undefined
                            }))
                            setPage(0);
                        }}
                    />
                </div>
            </div>
            <div className="mb-1 bg-gray p-3">
                <h5 className="text-center p-1">Видавець</h5>
                <div>
                    <Select className="rounded-all-0 text-dark"
                        isClearable
                        options={filterData ? filterData.publishers : undefined}
                        onChange={(value) => {
                            setSearchFilters(prevState => ({
                                ...prevState,
                                publisherId: value ? value.value : undefined
                            }))
                            setPage(0);
                        }}
                    />
                </div>
            </div>
            <div className="mb-1 bg-gray p-3">
                <h5 className="text-center p-1">Ціна</h5>
                <div className="p-3 d-flex justify-content-around gap-2">
                    <div className="bg-white">
                        <span className="px-1 text-dark">Від</span>
                        <input ref={minPrice}
                            className="border-0 price no-outline min-input" type="number"
                            min={0} max={100} step=".5"
                            defaultValue={searchFilters && searchFilters.priceFrom ? searchFilters.priceFrom : 0}
                            onChange={(e) => {
                                if (e.target.value.length === 0) {
                                    return
                                }
                                setSearchFilters(prevState => ({
                                    ...prevState,
                                    priceFrom: parseFloat(e.target.value)
                                }))
                                setPage(0);
                            }}
                        />
                    </div>
                    <div className="bg-white">
                        <span className="px-1 text-dark">До</span>
                        <input ref={maxPrice}
                            className="border-0 price no-outline max-input" type="number"
                            min={0} max={100} step=".5"
                            defaultValue={searchFilters && searchFilters.priceTo ? searchFilters.priceTo : 100}
                            onChange={(e) => {
                                if (e.target.value.length === 0) {
                                    return
                                }
                                setSearchFilters(prevState => ({
                                    ...prevState,
                                    priceTo: parseFloat(e.target.value)
                                }))
                                setPage(0);
                            }}
                        />
                    </div>
                </div>
            </div>
            <div className="mb-1 bg-gray p-3">
                <h5 className="text-center p-1">Дата виходу</h5>
                <div className="p-3 d-flex justify-content-around gap-2">
                    <div className="bg-white">
                        <span className="px-1 text-dark">Від</span>
                        <input ref={minDate}
                            className="border-0 no-outline min-input" type="number" min={1990} max={2100} step="1"
                            defaultValue={searchFilters && searchFilters.dateFrom ? searchFilters.dateFrom.getFullYear() : 1990}
                            onChange={(e) => {
                                setSearchFilters(prevState => ({
                                    ...prevState,
                                    dateFrom: new Date(e.target.value, 0, 1)
                                }))
                                setPage(0);
                            }}
                        />
                    </div>
                    <div className="bg-white">
                        <span className="px-1 text-dark">До</span>
                        <input ref={maxDate}
                            className="border-0 no-outline " type="number" min={1990} max={2100} step="1"
                            defaultValue={searchFilters && searchFilters.dateTo ? searchFilters.dateTo.getFullYear() : 2100}
                            onChange={(e) => {
                                setSearchFilters(prevState => ({
                                    ...prevState,
                                    dateTo: new Date(e.target.value, 0, 1)
                                }))
                                setPage(0);
                            }}
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
                            setPage(0);
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
                            setPage(0);
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
                            setPage(0);
                        }}
                    />
                    <label htmlFor="isHotOffer">Гарячі пропозиції</label>
                </div>
            </div>
            <button type="button" className="btn rounded-0 btn-outline-danger w-100 mb-1" onClick={() => reset()}>За замовчуванням</button>
        </div>
    )
}

export default FilterTable;