import axios from 'axios';

export async function getTagBySlug(slug) {
	const { data } = await axios.get(
		`${process.env.REACT_APP_API_ENDPOINT}/tags/byslug/${slug}`,
	);

	if (data.isSuccess) return data.result;
	else return null;
}

export async function getTagsByQueries(queries) {
	const { data } = await axios.get(
		`${process.env.REACT_APP_API_ENDPOINT}/tags?${queries}`,
	);

	if (data.isSuccess) return data.result;
	else return null;
}

export async function deleteTagById(id) {
	const { data } = await axios.delete(
		`${process.env.REACT_APP_API_ENDPOINT}/tags/${id}`,
	);

	return data;
}

export async function getTagById(id = 0) {
	const { data } = await axios.get(
		`${process.env.REACT_APP_API_ENDPOINT}/tags/${id}`,
	);

	if (data.isSuccess) return data.result;
	else return null;
}

export async function createTag(category) {
	const { data } = await axios.post(
		`${process.env.REACT_APP_API_ENDPOINT}/tags`,
		category,
	);

	return data;
}

export async function updateTag(id, category) {
	const { data } = await axios.put(
		`${process.env.REACT_APP_API_ENDPOINT}/tags/${id}`,
		category,
	);

	return data;
}
