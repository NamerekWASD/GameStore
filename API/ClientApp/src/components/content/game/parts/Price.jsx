
const Price = ({ item, priceClassName, discountClassName, vertical }) => {

    function renderDisountBlock() {
        return (
            item.discountPrice ?
                <span className="position-absolute text-decoration-line-through text-warm fs-6"
                    style={{ marginTop: '-50px', marginLeft: '-50px' }}>{item.price}$</span>
                :
                <>
                </>
        )
    }

    const getDiscountPercentage = () => {
        return `${((item.discountPrice / item.price * 100) - 100).toFixed(0)}%`
    }

    function renderPrice() {
        return (
            vertical ?
                <div className="d-flex mb-2 fw-bold">
                    {
                        item.discountPrice ?
                            <div className="d-flex border-gradient justify-content-center align-items-center"
                                style={{ width: '70px', height: '70px' }}>
                                <span className={"text-gradient " + discountClassName}
                                >
                                    {
                                        getDiscountPercentage()
                                    }
                                </span>
                            </div>
                            :
                            <> </>
                    }
                    <div className="d-flex flex-column align-items-center justify-content-center ms-3">
                        {
                            renderDisountBlock()
                        }
                        <span className={"p-1 fw-bold " + priceClassName}>{(item.discountPrice ?? item.price)}$</span>
                    </div>
                </div>
                :
                <div className="d-flex flex-row flex-wrap align-items-center gap-2 justify-content-center">
                    {
                        item.discountPrice ?
                            <>
                                <span className={"bg-discount p-2 text-white " + discountClassName}>
                                    {
                                        getDiscountPercentage()
                                    }
                                </span>
                            </>
                            :
                            <> </>
                    }
                    <span className={"bg-price p-2 text-white " + priceClassName}>{(item.discountPrice ?? item.price).toFixed(1)}$</span>
                </div>
        )
    }
    return (
        <>
            {
                item.isAvailable ?
                    <>
                        {
                            renderPrice()
                        }
                    </>
                    : ''
            }
        </>
    )
}
export default Price;