import axios from 'axios';

export async function getTagBySlug(slug) {
	const { data } = await axios.get(
		`${process.env.REACT_APP_API_ENDPOINT}/tags/${slug}`,
	);

	if (data.isSuccess) return data.result;
	else return null;
}
