import { useEffect, useMemo, useRef, useState } from "react";
import Creatable from 'react-select/creatable';
import Select from 'react-select';
import { loadFilterData, loadGameModel } from "../../../../utils/ApiRequests";
import Loading from "../../../../utils/Loading";
import { useSearchParams } from "react-router-dom";

const copyTypeBlank = {
    id: 0,
    name: '',
    platform: {
        id: 0,
        name: ''
    },
    availableRegions: [
        {
            id: 0,
            name: ''
        },
    ]
};
// const filterDataBlank = {
//     copyTypes: [copyTypeBlank],
//     platforms: [{ value: 0, label: '' }],
//     genres: [{ value: 0, label: '' }],
//     developers: [{ value: 0, label: '' }],
//     publishers: [{ value: 0, label: '' }],
//     availableRegions: [{ value: 0, label: '' }],
// };

const GameForm = ({ isCreate, saveChanges }) => {
    const [searchParams] = useSearchParams();
    const [needNewCopyType, setNeedNewCopyType] = useState(false);

    const [copyType, setCopyType] = useState(copyTypeBlank);

    const [filterData, setFilterData] = useState()
    const [game, setGame] = useState();
    const [isDiscounted, setDiscounted] = useState(false);

    useEffect(() => {
        if (isCreate) {
            setGame({});
        }
        if (!game && !isCreate) {
            loadGameModel(searchParams.get("gameId")).then(result => setGame(result));
        }
    }, [game, searchParams, isCreate]);

    useEffect(() => {
        setDiscounted(game && !isNaN(game.discountPrice) ? true : false)
    }, [game])


    useEffect(() => {
        if (!filterData) {
            loadFilterData().then(result => {
                setFilterData({
                    copyTypes: result.copyTypes.map(value => {
                        return {
                            label: value.name + ' ' + value.platform.name + ' | ' + value.availableRegions.map(item => item.name).join(', '),
                            value: value
                        };
                    }),
                    platforms: result.platforms.map(value => {
                        return {
                            label: value.name,
                            value: value.id
                        };
                    }),
                    genres: result.genres.map(value => {
                        return {
                            label: value.name,
                            value: value.id
                        };
                    }),
                    developers: result.developers.map(value => {
                        return {
                            label: value.name,
                            value: value.id
                        };
                    }),
                    publishers: result.publishers.map(value => {
                        return {
                            label: value.name,
                            value: value.id
                        };
                    }),
                    availableRegions: result.availableRegions.map(value => {
                        return {
                            label: value.name,
                            value: value.id
                        };
                    })
                })
            });
        }
    }, [filterData])


    const description = useRef(null);

    const submit = (e) => {
        e.preventDefault();
        const edited =  Object.assign({}, game);
        if (needNewCopyType) {
            edited.copyType = copyType;
            edited.description = description.current.value;
            edited.copyTypeId = 0;
        }
        saveChanges(edited);
    }

    const renderCopyType = () => {
        return (
            <>
                {
                    needNewCopyType ?
                        <div className=" bg-secondary p-3">
                            <div className="form-group required">
                                <label className="control-label text-white">Тип копії</label>
                                <input className="form-control rounded-0" type="text" id="copyTypeName" name="copyTypeName" onChange={(e) => setCopyType(prevState => ({
                                    ...prevState,
                                    name: e.target.value,
                                }))} />
                            </div>
                            <div className="form-group required">
                                <label className="control-label text-white">Платформа</label>
                                <Creatable className="rounded-all-0"
                                    options={filterData ? filterData.platforms : undefined}
                                    onChange={(value) => setCopyType(prevState => ({
                                        ...prevState,
                                        platformId: isNaN(value.value) ? 0 : value.value,
                                        platform: {
                                            id: isNaN(value.value) ? 0 : value.value,
                                            name: value.label,
                                        }
                                    }))}
                                />
                            </div>
                            <div className="form-group required">
                                <label className="control-label text-white">Доступні регіони</label>
                                <Creatable className="rounded-all-0"
                                    isMulti
                                    options={filterData ? filterData.availableRegions : undefined}
                                    onChange={(value) => setCopyType(prevState => ({
                                        ...prevState,
                                        availableRegions: value.map(region => { return { id: isNaN(region.value) ? 0 : region.value, name: region.label } })
                                    }
                                    ))} />
                            </div>
                        </div>
                        :
                        <div className="form-group required">
                            <label className="control-label">Тип копії</label>
                            <Select className="rounded-all-0"
                                options={filterData ? filterData.copyTypes : undefined}
                                onChange={(value) => setGame(prevState => ({
                                    ...prevState,
                                    copyTypeId: value.value.id,
                                    copyType: value.value
                                }))}
                                defaultValue={game && game.id ? {
                                    label: game.copyType.name + ' ' + game.copyType.platform.name + ' | ' + game.copyType.availableRegions.map(item => item.name).join(', '),
                                    value: game.copyType
                                } : {}}
                            />
                        </div>
                }
            </>
        )
    }
    const memoNewCopyType = useMemo(renderCopyType, [needNewCopyType, filterData, game]);

    const renderDiscount = () => {
        return (
            <>
                {
                    isDiscounted ?
                        <div className="form-group required">
                            <label className="control-label" htmlFor="gamePrice">Ціна зі знижкою</label>
                            <div className="d-flex border border-1">
                                <span className="my-auto ms-2">$</span>
                                <input type="number" name="gamePrice" id="gamePrice"
                                    className="form-control rounded-0 border-0 px-1"
                                    min={0.01} required step={0.01} defaultValue={game ? game.discountPrice : 0.01} max={game ? game.price : Infinity}
                                    onChange={(e) => setGame(prevState => (
                                        {
                                            ...prevState,
                                            discountPrice: e.target.value
                                        }
                                    ))} />
                            </div>
                            <button type="button" className="btn btn-outline-warning w-100 rounded-0"
                                onClick={(e) => {
                                    setGame(prevState => (
                                        {
                                            ...prevState,
                                            discountPrice: undefined
                                        }
                                    ))

                                    setDiscounted(!isDiscounted);
                                }}>Скасувати знижку</button>
                        </div>
                        :
                        <button type="button" className="btn btn-outline-primary w-100 rounded-0"
                            onClick={() => {
                                setGame(prevState => ({ ...prevState, discountPrice: game.price  }));
                                setDiscounted(!isDiscounted);
                            }}>Застосувати знижку</button>
                }
            </>
        )
    }

    const memoDiscount = useMemo(renderDiscount, [game, isDiscounted])

    const parseGameReleased = () => {
        let released = new Date(game.released);
        return released.toISOString().slice(0, 10);
    }
    return (
        <div className="container">
            {
                game ?
                    <form onSubmit={submit} className="bg-white p-3">
                        <h2 className="text-center">{isCreate ? "Створити" : "Оновити дані гри"}</h2>
                        <div className="form-group required">
                            <label className="control-label" htmlFor="id">ID</label>
                            <input className="form-control rounded-0" type="text" id="id" name="id" readOnly defaultValue={game ? game.id : 0} required />
                        </div>
                        <div className="form-group required">
                            <label className="control-label" htmlFor="title">Назва</label>
                            <input className="form-control rounded-0" type="text" id="title" name="title" defaultValue={game ? game.title : ''}
                                onChange={(e) => setGame(prevState => ({ ...prevState, title: e.target.value }))} required />
                        </div>
                        <div className="d-grid gap-3 grid-2">
                            <div>
                                <div className="form-group required">
                                    <label className="control-label">Розробник</label>
                                    <Creatable className="rounded-all-0"
                                        options={filterData ? filterData.developers : undefined}
                                        onChange={(value) => setGame(prevState => ({
                                            ...prevState,
                                            developerId: isNaN(value.value) ? 0 : value.value,
                                            developer: {
                                                id: isNaN(value.value) ? 0 : value.value,
                                                name: value.label
                                            }
                                        }))}
                                        defaultValue={game && game.id ? { label: game.developer.name, value: game.developer.id } : {}}
                                        required />
                                </div>
                                <div className="form-group required">
                                    <label className="control-label">Видавець</label>
                                    <Creatable className="rounded-all-0"
                                        options={filterData ? filterData.publishers : undefined}
                                        onChange={(value) => setGame(prevState => ({
                                            ...prevState,
                                            publisherId: isNaN(value.value) ? 0 : value.value,
                                            publisher: {
                                                id: isNaN(value.value) ? 0 : value.value,
                                                name: value.label
                                            }
                                        }))}
                                        defaultValue={game && game.id ? {
                                            label: game.publisher.name,
                                            value: game.publisher.id
                                        } : {}}
                                        required />
                                </div>
                                <div className="form-group required">
                                    <label className="control-label">Жанр</label>
                                    <Creatable className="rounded-all-0"
                                        isMulti
                                        options={filterData ? filterData.genres : undefined}
                                        onChange={(value) => setGame(prevState => ({
                                            ...prevState,
                                            genres: value.map(genre => { return { id: isNaN(genre.value) ? 0 : genre.value, name: genre.label } })
                                        }))}

                                        defaultValue={game && game.id ? game.genres.map(value => { return { label: value.name, value: value.id } }) : undefined}
                                        required />
                                </div>
                                <div className="form-group required">
                                    <label className="control-label" htmlFor="released">Дата релізу</label>
                                    <input type="date" name="released" id="released" className="form-control"
                                        onChange={(e) => setGame(prevState => ({ ...prevState, released: new Date(e.target.value).toISOString() }))}
                                        defaultValue={game && game.id ? parseGameReleased() : ''} />
                                </div>
                                <div className="form-group checkbox-unique">
                                    <input type="checkbox" name="isHotOffer" id="isHotOffer"
                                        defaultChecked={game ? game.isHotOffer : false}
                                        onChange={(e) => setGame(prevState => ({ ...prevState, isHotOffer: e.target.checked }))} />
                                    <label className="control-label fw-bold" htmlFor="isHotOffer">Гаряча пропозиція</label>
                                </div>
                                <div className="form-group checkbox-unique">
                                    <input type="checkbox" name="isAvailable" id="isAvailable"
                                        defaultChecked={game ? game.isAvailable : false}
                                        onChange={(e) => setGame(prevState => ({ ...prevState, isAvailable: e.target.checked }))} />
                                    <label className="control-label fw-bold" htmlFor="isAvailable">Доступно до продажу</label>
                                </div>
                            </div>
                            <div>
                                {
                                    memoNewCopyType
                                }
                                <button type="button"
                                    className="btn btn-outline-primary rounded-0 w-100 mb-3"
                                    onClick={() => setNeedNewCopyType(!needNewCopyType)} >
                                    {!needNewCopyType ? 'Створити інший тип копії' : 'Я передумав...'}
                                </button>
                                <div className="form-group required">
                                    <label className="control-label" htmlFor="gamePrice">Ціна</label>
                                    <div className="d-flex border border-1">
                                        <span className="my-auto ms-2">$</span>
                                        <input type="number" name="gamePrice" id="gamePrice"
                                            className="form-control rounded-0 border-0 px-1"
                                            min={0.01} required step={0.01} defaultValue={game ? game.price : 0.01}
                                            onChange={(e) => setGame(prevState => ({ ...prevState, price: e.target.value }))} />
                                    </div>
                                </div>
                                {
                                    memoDiscount
                                }
                            </div>
                        </div>
                        <div className="form-group required">
                            <label className="control-label" htmlFor="description">Опис</label>
                            <textarea ref={description}
                                className="w-100 form-control rounded-0"
                                name="description"
                                id="gameDescription"
                                rows="15"
                                defaultValue={game ? game.description : ''}>
                            </textarea>
                        </div>
                        <button className="btn btn-outline-success rounded-0 w-100" type="submit">{isCreate ? "Створити" : "Зберегти зміни"}</button>
                    </form>
                    :
                    <Loading />
            }
        </div>
    )
}

export default GameForm;