// A hyperlink anchor with an image attached to it.

type LinkedImgProps = {
    target: string; //The target URL
    source: string; //The image source
    imageClass?: string[]; //Stylistic class for the image
    imageID: string; //Stylistic ID for the image
    altText: string;
}

function LinkedImg({
    target,
    source,
    imageClass,
    imageID,
    altText,
}: LinkedImgProps) {
    return <a href={target} className={imageClass?.join(" ")} id={imageID}>
        <img src={source} alt={altText} />
        </a>
}

export default LinkedImg;