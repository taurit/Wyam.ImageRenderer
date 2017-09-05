# Wyam.ImageRenderer

## About the project
This project contains a custom module for Wyam that helps me build my blog. It's created to allow me better separate content from its presentation and avoid duplication of code related to displaying images.

## How it works?

This is a quick & dirty module to transform a custom markup similar to:

```html
<fig width="6"
        src="algorithms-to-live-by-cover.png"
        alt="Algorithms to Live By cover"
        caption="Book cover"
        source="Algorithms to Live By"
        source-link="http://algorithmstoliveby.com/"
/>
```

into a HTML code like:

```html
<div class="row" aria-hidden="true">
    <figure class="col-md-6 col-md-offset-3">
        <div class="thumbnail">
            <picture>
                <source srcset="algorithms-to-live-by-cover.webp" type="image/webp">
                <source srcset="algorithms-to-live-by-cover.png" type="image/png">

                <img class="img-responsive img-generated" src="/assets/img/posts/png/algorithms-to-live-by-cover.png" alt="Algorithms to Live By cover">
            </picture>

            <div class="caption">
                <figcaption>
                    Fig. 1. Book cover. Source: <a href="http://algorithmstoliveby.com/" rel="nofollow" target="_blank">Algorithms to Live By</a>
                </figcaption>
            </div>
        </div>
    </figure>
</div>
```

## Project status

This project is not intended for use by other Wyam users, as it's tightly tailored for my workflow and conventions.

It might, however, serve as an example of a module transforming some HTML fragment into another. I couldn't find any other example doing it, and hooking into Wyam was certainly the most challenging part.