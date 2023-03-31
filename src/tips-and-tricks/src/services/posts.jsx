import axios from 'axios';

export async function getPostsByQueries(queries) {
	const response = await axios.get(
		`https://localhost:7157/api/posts?${queries}`,
	);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}
