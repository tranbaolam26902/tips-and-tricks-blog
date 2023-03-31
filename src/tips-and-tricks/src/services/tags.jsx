import axios from 'axios';

export async function getTagBySlug(slug) {
	const response = await axios.get(`https://localhost:7157/api/tags/${slug}`);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}
