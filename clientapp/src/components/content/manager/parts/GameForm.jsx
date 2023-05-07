import { useEffect, useRef, useState } from "react";
import Creatable from 'react-select/creatable';
import Select from 'react-select';
import { deleteGame, GetFilterData, GetGameModel, sendCopyData, uploadImageToServer } from "../../../../utils/ApiRequests";
import Loading from "../../../../utils/Loading";
import { useNavigate, useSearchParams } from "react-router-dom";
import LoadingCircle from "../../../../utils/LoadingCircle";
import { POSTER } from "../../../../utils/Constants";
import { toast } from "react-toastify";
import ImageModal from "../../game/parts/ImageModal";
import $ from 'jquery';
import ModalDelete from "./ModalDelete";
import { AppPaths } from "../../../../utils/AppPaths";

const GameForm = ({ saveChanges }) => {
    const navigate = useNavigate();
    const [searchParams] = useSearchParams();
    const [needNewCopyType, setNeedNewCopyType] = useState(false);

    const [copyType, setCopyType] = useState();

    const [filterData, setFilterData] = useState()
    const [game, setGame] = useState();
    const [addImage, setAddImage] = useState(false);
    const [newImage, setNewImage] = useState({
        typeId: 1,
        type: {
            id: 1,
            name: POSTER
        }
    });
    const [isDiscounted, setDiscounted] = useState(false);
    const [description, setDescription] = useState();
    const [showImageModal, setShowImageModal] = useState(false);
    const [imageToShow, setImageToShow] = useState();

    const modalRef = useRef(null);

    useEffect(() => {
        if (!game) {
            const id = searchParams.get("gameId");
            if (!id) {
                setGame({ id: 0 });
                return;
            }
            GetGameModel(id).then(response => {
                if (response.status === 200) {
                    return response.json()
                }
            })
                .then(result => {
                    if (result) {
                        setGame(result)
                        setDescription(result.description ?? '')
                    }
                });
        }
    }, [game, searchParams]);

    useEffect(() => {
        setDiscounted(game && game.discountPrice !== null ? true : false)
    }, [game])

    useEffect(() => {
        GetFilterData().then(result => {
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
                }),
                imageTypes: result.imageTypes.map(value => {
                    return {
                        label: value.name,
                        value: value.id
                    };
                }),
                tags: result.tags.map(value => {
                    return {
                        label: value.name,
                        value: value.id
                    };
                }),
            })
        });
    }, [])

    const submit = (e) => {
        e.preventDefault();
        const image = game.images.some(value => value.type.name === POSTER)
        if (!image) {
            toast.error(`В продукті має бути хоча б одне зображення із назвою "${POSTER}"`);
            return;
        }

        const edited = Object.assign({}, game);
        if (needNewCopyType) {
            edited.copyType = copyType;
            edited.copyTypeId = 0;
        }
        edited.description = description;
        saveChanges(edited);
    }

    const renderCreateCopy = () => {
        return (
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
        )
    }

    const parseGameReleased = () => {
        let released = new Date(game.released);
        return released.toISOString().slice(0, 10);
    }

    const scrollToBottom = () => {
        const element = $("#scroll-to-bottom");
        element.animate({
            scrollTop: element.prop("scrollHeight")
        }, 500)
    }

    const insertImage = () => {
        setAddImage(false);
        if (game.images) {
            game.images.push(newImage);
            return;
        }
        setGame(prevState => ({
            ...prevState,
            images: [newImage]
        }));
        scrollToBottom()
    }

    const uploadImage = async (e) => {
        const image = new FormData();
        setAddImage(false);
        image.append('typeId', newImage.typeId);
        image.append('type', newImage.type.name);
        image.append('image', e.target.files[0]);
        const response = await uploadImageToServer(image, game.id);
        if (!response.ok) {
            toast.error(await response.text());
            return;
        }
        const newArray = game.images;
        newArray.push(await response.json())
        setGame(prevState => ({
            ...prevState,
            images: newArray
        }));
        scrollToBottom()
    }

    const showModal = (image) => {
        setImageToShow(image);
        setShowImageModal(true);
    }

    const handleDelete = () => {
        modalRef.current.style.display = 'block';
    }

    const acceptDelete = async () => {
        const response = await deleteGame(game.id);
        if (response.status !== 200) {
            toast.error(await response.text());
            return;
        }
        toast.success("Продукт успішно видалений!");
        navigate(AppPaths.manager);
    }
    const copyData = useRef(null);

    const insertCopy = async () => {
        const data = { id: 0, data: copyData.current.value, gameId: game.id, game: undefined };
        const response = await sendCopyData(data);
        if (response.status === 200) {
            toast.success("Ви успішно додали копію!");
            copyData.current.value = '';
            return;
        }
        toast.error("Сталась помилка...");
    }

    return (
        <div className="container">
            {
                game ?
                    <form onSubmit={submit} className="bg-white p-3">
                        <h2 className="text-center">{game && game.id !== 0 ? "Оновити дані гри" : "Створити гру"}</h2>
                        <div className="form-group required">
                            <label className="control-label" htmlFor="id">ID</label>
                            <input className="form-control rounded-0" type="text" id="id" name="id" readOnly defaultValue={game ? game.id : 0} required />
                        </div>
                        <div className="form-group required">
                            <label className="control-label" htmlFor="title">Назва</label>
                            <input className="form-control rounded-0" type="text" id="title" name="title" defaultValue={game ? game.title : ''}
                                onChange={(e) => setGame(prevState => ({ ...prevState, title: e.target.value }))} required />
                        </div>
                        <div className="game-form grid-3 ">
                            <div className="border p-2">
                                <h4>Загальна інформація</h4>
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
                                        defaultValue={game && game.id !== 0 ? { label: game.developer.name, value: game.developer.id } : {}}
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
                                        defaultValue={game && game.id !== 0 ? {
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

                                        defaultValue={game && game.id !== 0 ? game.genres.map(value => { return { label: value.name, value: value.id } }) : undefined}
                                        required />
                                </div>
                                <div className="form-group required">
                                    <label className="control-label">Мітки</label>
                                    <Creatable className="rounded-all-0"
                                        isMulti
                                        options={filterData ? filterData.tags : undefined}
                                        onChange={(value) => setGame(prevState => ({
                                            ...prevState,
                                            tags: value.map(tag => { return { id: isNaN(tag.value) ? 0 : tag.value, name: tag.label } })
                                        }))}

                                        defaultValue={game && game.id !== 0 ? game.tags.map(value => { return { label: value.name, value: value.id } }) : undefined}
                                        required />
                                </div>
                                <div className="form-group required">
                                    <label className="control-label" htmlFor="released">Дата релізу</label>
                                    <input type="date" name="released" id="released" className="form-control"
                                        onChange={(e) => setGame(prevState => ({ ...prevState, released: new Date(e.target.value).toISOString() }))}
                                        defaultValue={game && game.id !== 0 ? parseGameReleased() : ''} />
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
                            <div className="border p-2">
                                <h4>Копії</h4>
                                {
                                    needNewCopyType ?
                                        renderCreateCopy()
                                        :
                                        <div className="form-group ">
                                            <label className="control-label">Тип копії</label>
                                            <Select className="rounded-all-0"
                                                isClearable
                                                options={filterData ? filterData.copyTypes : undefined}
                                                onChange={(value) => {
                                                    if (value) {
                                                        setGame(prevState => ({
                                                            ...prevState,
                                                            copyTypeId: value.value.id,
                                                            copyType: value.value
                                                        }))
                                                        return;
                                                    }
                                                    setGame(prevState => ({
                                                        ...prevState,
                                                        copyTypeId: undefined,
                                                        copyType: undefined
                                                    }))
                                                }}
                                                defaultValue={game && game.id !== 0 && game.copyType ? {
                                                    label: game.copyType.name + ' ' + game.copyType.platform.name + ' | ' + game.copyType.availableRegions.map(item => item.name).join(', '),
                                                    value: game.copyType
                                                } : undefined}
                                            />
                                        </div>
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
                                            min={0.01} required step={0.01} defaultValue={game && game.id !== 0 ? game.price : 0.01}
                                            onChange={(e) => setGame(prevState => ({ ...prevState, price: e.target.value }))} />
                                    </div>
                                </div>
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
                                                            discountPrice: null
                                                        }
                                                    ))

                                                    setDiscounted(!isDiscounted);
                                                }}>Скасувати знижку</button>
                                        </div>
                                        :
                                        <button type="button" className="btn btn-outline-primary w-100 rounded-0"
                                            onClick={() => {
                                                setGame(prevState => ({ ...prevState, discountPrice: game.price }));
                                                setDiscounted(!isDiscounted);
                                            }}>Застосувати знижку</button>
                                }
                            </div>
                            <div className="border p-2">
                                <h4>Зображення</h4>
                                <div id="scroll-to-bottom" className="border border-1 border-dark overflow-scroll hide-scrollbar" style={{ height: '50vh' }}>
                                    {
                                        game && game.images ? game.images.map((image, index) => {
                                            return (
                                                <div key={image.id ?? index - 99999} className="p-4">
                                                    <div className="position-relative">
                                                        <span className="position-absolute close-modal text-center bg-danger"
                                                            style={{ width: '50px', height: '50px', transition: '.3s', right: '0' }}
                                                            onClick={() =>
                                                                setGame(prevState => (
                                                                    {
                                                                        ...prevState,
                                                                        images: game.images.filter(img => img.id !== image.id)
                                                                    }
                                                                ))}>&times;</span>
                                                        <img className="w-100 pointer" src={image.path} alt={image.name}
                                                            onClick={() => showModal(image)} />
                                                    </div>
                                                </div>
                                            )
                                        })
                                            :
                                            <div className="text-center w-100">
                                                <LoadingCircle />
                                            </div>
                                    }
                                </div>
                                {
                                    addImage ?
                                        <div>
                                            <div className="form-group required">
                                                <label className="control-label" htmlFor="pic-name">Тип зображення</label>
                                                <Creatable className="rounded-all-0"
                                                    options={filterData ? filterData.imageTypes : undefined}
                                                    onChange={(value) => {
                                                        setNewImage(prevState => ({
                                                            ...prevState,
                                                            typeId: isNaN(value.value) ? 0 : value.value,
                                                            type: {
                                                                id: isNaN(value.value) ? 0 : value.value,
                                                                name: value.label
                                                            }
                                                        }))
                                                    }}
                                                    defaultValue={{ label: POSTER, value: 1 }}
                                                    required />
                                            </div>
                                            <hr />
                                            <div className="form-group required">
                                                <label className="control-label" htmlFor="add-hyperlink">Додати посилання</label>
                                                <input type="text" name="add-hyperlink" id="game-image-h" className="rounded-0 form-control"
                                                    placeholder={'https://content.com/...'} onChange={(e) => setNewImage(prevState => ({ ...prevState, path: e.target.value }))} />
                                            </div>
                                            <button type="button" className="btn btn-outline-success rounded-0 w-100 mb-2" onClick={() => insertImage()}>Прикріпити</button>
                                            <div className="or">Або</div>
                                            <div className="form-group">
                                                <label className="control-label" htmlFor="upload-on-server">Завантажити на сервер</label>
                                                <input type="file" name="upload-on-server" id="game-image" className="rounded-0 form-control"
                                                    accept="image/*" onChange={(e) => uploadImage(e)} disabled={game && game.id === 0} />
                                            </div>
                                        </div>
                                        :

                                        <button type="button" className="btn btn-outline-secondary w-100 rounded-0"
                                            onClick={() => setAddImage(true)}>Додати зображення</button>
                                }
                            </div>
                        </div>

                        <div className="d-flex gap-3 mb-3">
                            <div className="form-group required w-100">
                                <h4 className="control-label fw-bold">Опис</h4>
                                <textarea
                                    className=" form-control rounded-0"
                                    name="description"
                                    id="gameDescription"
                                    rows="10"
                                    defaultValue={description ? description : ''}
                                    onChange={(e) => setDescription(e.target.value)}>
                                </textarea>
                            </div>
                            <div className="w-100">
                                <h4 className="control-label fw-bold">Вигляд опису на сторінці</h4>
                                <div className="text-white bg-dark p-3">
                                    <h2>Опис</h2>
                                    <div dangerouslySetInnerHTML={description ? { __html: description } : { __html: '' }}>

                                    </div>
                                </div>
                            </div>
                        </div>
                        {
                            game && game.id !== 0 ?
                                <div>
                                    <h3>Заповніть поле даними копії</h3>
                                    <div>
                                        <textarea ref={copyData} className="form-control rounded-0" name="copy-data" id="copy-data" rows="3"></textarea>
                                    </div>
                                    <button type="button" className="btn btn-outline-primary rounded-0 w-100" onClick={insertCopy}>Завантажити копію</button>
                                </div>
                                : ''
                        }
                        <hr />
                        <button className="btn btn-outline-success rounded-0 w-100" type="submit">{"Зберегти зміни"}</button>
                        {
                            game && game.id !== 0 ?
                                <>
                                    <hr />
                                    <button type="button" className="btn btn-outline-danger rounded-0 w-100" onClick={() => handleDelete()}>Видалити продукт</button>
                                    <ModalDelete refModal={modalRef} onAcceptDelete={acceptDelete} game={game} />
                                </>
                                :
                                ''
                        }
                    </form>
                    :
                    <Loading />
            }
            {
                game && game.images ?
                    <ImageModal
                        images={game.images}
                        current={imageToShow ?? game.images[0]}
                        setImageToShow={setImageToShow}
                        showContent={showImageModal}
                        setShowContent={setShowImageModal} />
                    : ''
            }
        </div>
    )
}

export default GameForm;