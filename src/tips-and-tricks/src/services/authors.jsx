import axios from 'axios';

export async function getAuthorBySlug(slug) {
	const response = await axios.get(
		`https://localhost:7157/api/authors/${slug}`,
	);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}
